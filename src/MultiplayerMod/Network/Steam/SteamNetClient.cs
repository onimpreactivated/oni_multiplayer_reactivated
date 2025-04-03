using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Exceptions;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.Network.Common;
using MultiplayerMod.Network.Common.Components;
using MultiplayerMod.Network.Common.Interfaces;
using MultiplayerMod.Network.Common.Message;
using Steamworks;
using UnityEngine;
using static Steamworks.Constants;
using static Steamworks.ESteamNetConnectionEnd;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Creates a new Steam Networking Client
/// </summary>
/// <remarks>
/// Creates a new Steam Networking Client
/// </remarks>
/// <param name="lobby"></param>
public class SteamNetClient(SteamLobby lobby) : INetClient
{
    /// <inheritdoc/>
    public INetId Id => new SteamNetId(SteamUser.GetSteamID());

    /// <inheritdoc/>
    public NetStateClient State { get; private set; } = NetStateClient.Disconnected;

    /// <inheritdoc/>
    public event Action<NetStateClient> StateChanged;

    /// <inheritdoc/>
    public event Action<BaseCommandEvent> CommandReceived;

    private readonly SteamLobby lobby = lobby;
    private readonly NetworkMessageFragmentProcessor messageProcessor = new();

    private HSteamNetConnection connection = HSteamNetConnection.Invalid;
    private readonly SteamNetworkingConfigValue_t[] networkConfig = [SteamConfiguration.SendBufferSize()];
    private GameObject gameObject;

    /// <inheritdoc/>
    public void Connect(IEndPoint endpoint)
    {
        if (!SteamManager.Initialized)
            return;

        if (endpoint is not SteamServerEndpoint steamServerEndpoint)
        {
            Debug.LogError("Join endpoint is not for steam!");
            return;
        }

        SetState(NetStateClient.Connecting);

        SteamNetworkingUtils.GetRelayNetworkStatus(out var status);
        if (status.m_eAvail != ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Current)
            SteamNetworkingUtils.InitRelayNetworkAccess();

        if (lobby.Connected)
        {
            OnLobbyJoin();
            return;
        }

        lobby.OnJoin += OnLobbyJoin;
        lobby.Join(steamServerEndpoint.LobbyID);
    }

    /// <inheritdoc/>
    public void Disconnect()
    {
        if (State == NetStateClient.Disconnected)
            throw new NetworkPlatformException("Client not connected");

        UnityObjectManager.Destroy(gameObject);
        lobby.Leave();
        lobby.OnJoin -= OnLobbyJoin;
        SteamNetworkingSockets.CloseConnection(connection, (int) k_ESteamNetConnectionEnd_App_Generic, "", true);
        SetState(NetStateClient.Disconnected);
        SteamFriends.ClearRichPresence();
    }

    /// <inheritdoc/>
    public void Tick()
    {
        if (State != NetStateClient.Connected)
            return;

        SteamNetworkingSockets.RunCallbacks();
        ReceiveCommands();
    }

    /// <inheritdoc/>
    public void Send(BaseCommandEvent command, MultiplayerCommandOptions options = MultiplayerCommandOptions.None)
    {
        Debug.Log($"Client sending: {command}");
        if (State != NetStateClient.Connected)
            throw new NetworkPlatformException("Client not connected");

        foreach (INetworkMessageHandle handle in NetworkMessageHelper.Create(command, options))
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

            EResult result = SteamNetworkingSockets.SendMessageToConnection(
                    connection,
                    steamHandle.Pointer,
                    steamHandle.Size,
                    k_nSteamNetworkingSend_Reliable,
                    out long messageOut
                );
            if (result != EResult.k_EResultOK || messageOut == 0)
            {
                Debug.LogError($"Failed to send {command}: {result}");
                SetState(NetStateClient.Error);
            }
            Debug.Log($"Finished sending! Result: {result} MessageOut: {messageOut} ");
            handle.Dispose();
            steamHandle.Dispose();
        }
    }

    private void SetState(NetStateClient status)
    {
        Debug.Log("NetStateClient: " + status);
        State = status;
        StateChanged?.Invoke(status);
    }

    private void OnLobbyJoin()
    {
        var serverId = lobby.GameServerId;
        if (serverId == CSteamID.Nil)
        {
            Debug.Log("Unable to get lobby game server");
            SetState(NetStateClient.Error);
            return;
        }
        Debug.Log($"Lobby game server is {serverId}");
        var identity = GetNetworkingIdentity(serverId);
        connection = SteamNetworkingSockets.ConnectP2P(ref identity, 0, networkConfig.Length, networkConfig);

        Debug.Log($"P2P Connect to {serverId}");

        SetRichPresence();

        gameObject = UnityObjectManager.CreateStaticWithComponent<ClientComponent>();
        gameObject.GetComponent<ClientComponent>().client = this;
        SetState(NetStateClient.Connected);
    }

    private void SetRichPresence()
    {
        SteamFriends.SetRichPresence("connect", $"+connect_lobby {lobby.Id}");
    }

    private SteamNetworkingIdentity GetNetworkingIdentity(CSteamID steamId)
    {
        SteamNetworkingIdentity identity = new();
        identity.SetSteamID(steamId);
        return identity;
    }

    private void ReceiveCommands()
    {
        IntPtr[] messages = new IntPtr[128];
        int messagesCount = SteamNetworkingSockets.ReceiveMessagesOnConnection(connection, messages, 128);
        for (int i = 0; i < messagesCount; i++)
        {
            SteamNetworkingMessage_t steamMessage = SteamNetworkingMessage_t.FromIntPtr(messages[i]);
            NetworkMessage message = messageProcessor.Process(steamMessage.m_conn.m_HSteamNetConnection, steamMessage.GetNetworkMessageHandle());
            if (message != null)
            {
                Debug.Log($"Client Processed message: {message}");
                if (!message.Options.HasFlag(MultiplayerCommandOptions.OnlyHost))
                    CommandReceived?.Invoke(message.CommandEvent);

                // Easy dispose
                if (message is IDisposable ds && ds != null)
                    ds.Dispose();
            }
            SteamNetworkingMessage_t.Release(messages[i]);
        }
    }

}
