using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Exceptions;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.Extensions;
using MultiplayerMod.Network.Common;
using MultiplayerMod.Network.Common.Components;
using MultiplayerMod.Network.Common.Interfaces;
using MultiplayerMod.Network.Common.Message;
using Steamworks;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using UnityEngine;
using static Steamworks.Constants;
using static Steamworks.EResult;
using static Steamworks.ESteamNetConnectionEnd;
using static Steamworks.ESteamNetworkingConnectionState;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Creates a new Steam Networking Server
/// </summary>
/// <remarks>
/// Creates a new Steam Networking Server
/// </remarks>
/// <param name="lobby"></param>
public class SteamNetServer(SteamLobby lobby) : INetServer
{
    /// <inheritdoc/>
    public NetStateServer State { private set; get; } = NetStateServer.Stopped;

    /// <inheritdoc/>
    public IEndPoint Endpoint
    {
        get
        {
            if (State != NetStateServer.Started)
                throw new NetworkPlatformException("Server isn't started");

            return new SteamServerEndpoint(lobby.Id);
        }
    }

    /// <inheritdoc/>
    public List<INetId> Clients => new(clients.Select(it => it.Key));

    /// <inheritdoc/>
    public event Action<NetStateServer> StateChanged;
    /// <inheritdoc/>
    public event Action<INetId> ClientConnected;
    /// <inheritdoc/>
    public event Action<INetId> ClientDisconnected;
    /// <inheritdoc/>
    public event Action<INetId, BaseCommandEvent> CommandReceived;

    private Callback<SteamServersConnected_t> steamServersConnectedCallback = null!;
    private TaskCompletionSource<bool> lobbyCompletionSource = null!;
    private TaskCompletionSource<bool> steamServersCompletionSource = null!;
    private CancellationTokenSource callbacksCancellationTokenSource = null!;

    private HSteamNetPollGroup pollGroup;
    private HSteamListenSocket listenSocket;
    private readonly NetworkMessageFragmentProcessor messageProcessor = new();
    private readonly SteamNetworkingConfigValue_t[] networkConfig = [SteamConfiguration.SendBufferSize()];
    private Callback<SteamNetConnectionStatusChangedCallback_t> connectionStatusChangedCallback = null!;

    private readonly Dictionary<INetId, HSteamNetConnection> clients = [];
    private readonly INetId currentPlayer = new SteamNetId(SteamUser.GetSteamID());

    //private readonly UnityTaskScheduler scheduler;
    //private readonly MultiplayerCommandRegistry commands;
    private readonly SteamLobby lobby = lobby;

    private GameObject gameObject;

    /// <inheritdoc/>
    public void Send(INetId clientId, BaseCommandEvent command)
    {
        if (!clients.ContainsKey(clientId))
            return;
        ReadOnlyCollection<HSteamNetConnection> connections = new([clients[clientId]]);
        SendCommand(command, MultiplayerCommandOptions.None, connections);
    }

    /// <inheritdoc/>
    public void Send(BaseCommandEvent command, MultiplayerCommandOptions options)
    {
        IEnumerable<KeyValuePair<INetId, HSteamNetConnection>> recipients = clients;
        if (options.HasFlag(MultiplayerCommandOptions.SkipHost))
            recipients = recipients.Where(entry => !entry.Key.Equals(currentPlayer));

        SendCommand(command, options, recipients.Select(it => it.Value));
    }

    /// <inheritdoc/>
    public void Start()
    {
        if (!SteamManager.Initialized)
            throw new NetworkPlatformException("Steam API is not initialized");

        Debug.Log("Starting...");
        SetState(NetStateServer.Preparing);
        try
        {
            Initialize();
        }
        catch (Exception ex)
        {
            Debug.Log($"Initialization error: {ex}");
            Reset();
            SetState(NetStateServer.Error);
            throw;
        }
        gameObject = UnityObjectManager.CreateStaticWithComponent<ServerComponent>();
        gameObject.GetComponent<ServerComponent>().server = this;
    }

    /// <inheritdoc/>
    public void Stop()
    {
        if (State <= NetStateServer.Stopped)
            throw new NetworkPlatformException("Server isn't started");

        Debug.Log("Stopping...");
        if (gameObject != null)
            UnityObjectManager.Destroy(gameObject);
        Reset();
        SetState(NetStateServer.Stopped);
    }

    /// <inheritdoc/>
    public void Tick()
    {
        switch (State)
        {
            case NetStateServer.Starting:
            case NetStateServer.Started:
                GameServer.RunCallbacks();
                SteamGameServerNetworkingSockets.RunCallbacks();
                ReceiveMessages();
                break;
        }
    }

    private void SendCommand(BaseCommandEvent command, MultiplayerCommandOptions options, IEnumerable<HSteamNetConnection> connections)
    {
        var sequence = NetworkMessageHelper.Create(command, options);
        sequence.ForEach(handle =>
        {
            connections.ForEach(connection => Send(handle, connection));
            handle.Dispose();
        });

    }

    private void Send(INetworkMessageHandle handle, HSteamNetConnection connection)
    {
        ISteamNetworkMessageHandle steamHandle;
        if (handle is not ISteamNetworkMessageHandle)
        {
            steamHandle = new SteamNetworkMessageHandle(handle);
        }
        else
        {
            steamHandle = (ISteamNetworkMessageHandle) handle;
        }

        EResult result = SteamGameServerNetworkingSockets.SendMessageToConnection(
            connection,
            steamHandle.Pointer,
            steamHandle.Size,
            k_nSteamNetworkingSend_Reliable,
            out _
        );
        if (result != k_EResultOK && result != k_EResultNoConnection)
            Debug.LogError($"Failed to send message, result: {result}");
    }

    private void Initialize()
    {
        steamServersConnectedCallback = Callback<SteamServersConnected_t>
            .CreateGameServer(_ => ConnectedToSteamCallback());

        lobbyCompletionSource = new TaskCompletionSource<bool>();
        steamServersCompletionSource = new TaskCompletionSource<bool>();
        callbacksCancellationTokenSource = new CancellationTokenSource();
        Task.WhenAll(lobbyCompletionSource.Task, steamServersCompletionSource.Task)
            .ContinueWith(
                _ => OnServerStarted(),
                TaskContinuationOptions.None
            );

        lobby.OnCreate += OnLobbyCreated;
        lobby.Create();

        var version = typeof(SteamNetServer).Assembly.GetName().Version.ToString();
        Debug.Log($"Initializing game server version {version}");
        if (!GameServer.Init(0, 27020, 27015, EServerMode.eServerModeNoAuthentication, version))
            throw new NetworkPlatformException("Game server init failed");
        SteamGameServer.SetModDir("OxygenNotIncluded");
        SteamGameServer.SetProduct("OxygenNotIncluded Multiplayer");
        SteamGameServer.SetGameDescription("OxygenNotIncluded Multiplayer");

        SteamGameServer.LogOnAnonymous();

        SteamNetworkingUtils.GetRelayNetworkStatus(out var status);
        if (status.m_eAvail != ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Current)
            SteamNetworkingUtils.InitRelayNetworkAccess();

        connectionStatusChangedCallback = Callback<SteamNetConnectionStatusChangedCallback_t>
            .CreateGameServer(HandleConnectionStatusChanged);

        listenSocket = SteamGameServerNetworkingSockets.CreateListenSocketP2P(0, networkConfig.Length, networkConfig);
        pollGroup = SteamGameServerNetworkingSockets.CreatePollGroup();

        SetState(NetStateServer.Starting);
    }

    private void Reset()
    {
        lobby.OnCreate -= OnLobbyCreated;
        lobby.Leave();

        connectionStatusChangedCallback.Unregister();
        SteamGameServerNetworkingSockets.DestroyPollGroup(pollGroup);
        SteamGameServerNetworkingSockets.CloseListenSocket(listenSocket);
        SteamGameServer.LogOff();

        GameServer.Shutdown();

        steamServersConnectedCallback.Unregister();

        lobbyCompletionSource.TrySetCanceled();
        steamServersCompletionSource.TrySetCanceled();
        callbacksCancellationTokenSource.Cancel();
    }

    private void SetState(NetStateServer state)
    {
        Debug.Log("NetStateServer: " + state);
        State = state;
        StateChanged?.Invoke(state);
    }

    private void HandleConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t data)
    {
        HSteamNetConnection connection = data.m_hConn;
        CSteamID clientSteamId = data.m_info.m_identityRemote.GetSteamID();
        ESteamNetworkingConnectionState state = data.m_info.m_eState;
        switch (state)
        {
            case k_ESteamNetworkingConnectionState_Connecting:
                if (TryAcceptConnection(connection, clientSteamId))
                    ClientConnected?.Invoke(new SteamNetId(clientSteamId));
                break;
            case k_ESteamNetworkingConnectionState_ProblemDetectedLocally:
            case k_ESteamNetworkingConnectionState_ClosedByPeer:
                CloseConnection(connection, clientSteamId);
                break;
            case k_ESteamNetworkingConnectionState_Connected:
                Debug.Log($"Client {clientSteamId} successfully connected!");
                break;
            default:
                Debug.Log($"Unhandled connection state change {data.m_eOldState} -> {state} for client {clientSteamId}");
                break;
        }
    }

    private bool TryAcceptConnection(HSteamNetConnection connection, CSteamID clientSteamId)
    {
        EResult result = SteamGameServerNetworkingSockets.AcceptConnection(connection);
        if (result != k_EResultOK)
        {
            Debug.LogError($"Unable to accept connection from {clientSteamId} (error {result})");
            var reason = (int) k_ESteamNetConnectionEnd_AppException_Generic + (int) result;
            SteamGameServerNetworkingSockets.CloseConnection(
                connection,
                reason,
                pszDebug: $"Failed to accept connection (error {result})",
                bEnableLinger: false
            );
            return false;
        }
        SteamGameServerNetworkingSockets.SetConnectionPollGroup(connection, pollGroup);
        clients.Add(new SteamNetId(clientSteamId), connection);
        Debug.Log($"Connection accepted from {clientSteamId}");
        return true;
    }

    private void CloseConnection(HSteamNetConnection connection, CSteamID clientSteamId)
    {
        ClientDisconnected?.Invoke(new SteamNetId(clientSteamId));
        SteamGameServerNetworkingSockets.CloseConnection(
            connection,
            (int) k_ESteamNetConnectionEnd_App_Generic,
            null,
            false
        );
        clients.Remove(new SteamNetId(clientSteamId));
        Debug.Log($"Connection closed for {clientSteamId}");
    }

    private void OnServerStarted()
    {
        lobby.GameServerId = SteamGameServer.GetSteamID();
        SetState(NetStateServer.Started);
    }

    private void OnLobbyCreated()
    {
        SteamMatchmaking.SetLobbyData(lobby.Id, "server.name", $"{SteamFriends.GetPersonaName()}");
        lobbyCompletionSource.SetResult(true);
    }

    private void ConnectedToSteamCallback() => steamServersCompletionSource.SetResult(true);

    private void ReceiveMessages()
    {
        var messages = new IntPtr[128];
        var messagesCount = SteamGameServerNetworkingSockets.ReceiveMessagesOnPollGroup(pollGroup, messages, 128);
        for (var i = 0; i < messagesCount; i++)
        {
            var steamMessage = Marshal.PtrToStructure<SteamNetworkingMessage_t>(messages[i]);
            var message = messageProcessor.Process(
                steamMessage.m_conn.m_HSteamNetConnection,
                steamMessage.GetNetworkMessageHandle()
            );
            if (message != null)
            {
                Debug.Log($"Server Processed message: {message}");
                INetId id = new SteamNetId(steamMessage.m_identityPeer.GetSteamID());
                if (message.Options.HasFlag(MultiplayerCommandOptions.OnlyHost))
                {
                    this.CommandReceived?.Invoke(id, message.CommandEvent);
                }
                else
                {
                    var connections = clients.Where(it => !it.Key.Equals(id)).Select(it => it.Value);
                    SendCommand(message.CommandEvent, message.Options, connections);
                }

                // Easy dispose
                if (message is IDisposable ds && ds != null)
                    ds.Dispose();
            }
            SteamNetworkingMessage_t.Release(messages[i]);
        }
    }

}
