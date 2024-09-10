using MultiplayerMod.Core.Logging;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.Multiplayer.Commands;
using MultiplayerMod.Network;
using MultiplayerMod.Platform.Common.Network.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using MultiplayerMod.Platform.Common.Network;
using MultiplayerMod.ModRuntime.StaticCompatibility;
using MultiplayerMod.Multiplayer.Commands.Registry;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Linq;
using MultiplayerMod.Platform.Common.Network.Messaging;
using MultiplayerMod.Core.Extensions;
using MultiplayerMod.Core.Collections;
using System.Collections.Concurrent;
using MultiplayerMod.Multiplayer.Commands.Player;
namespace MultiplayerMod.Platform.Lan.Network;
public record ServerEndpoint : IMultiplayerEndpoint ;

internal class LanServer : IMultiplayerServer {
    public static LanServer? instance = null;

    public MultiplayerServerState State { private set; get; } = MultiplayerServerState.Stopped;
    public IMultiplayerEndpoint Endpoint => new ServerEndpoint();

    public List<IMultiplayerClientId> Clients => new(clientIdToLanClient.Select(it => it.Key));

    public event Action<MultiplayerServerState>? StateChanged;
    public event Action<IMultiplayerClientId>? ClientConnected;
    public event Action<IMultiplayerClientId>? ClientDisconnected;
    public event Action<IMultiplayerClientId, IMultiplayerCommand>? CommandReceived;

    private readonly Dictionary<string, LanMultiplayerClientId> lanClientIdToClientId = new();
    private readonly Dictionary<LanMultiplayerClientId, LanServerClient> clientIdToLanClient = new();

    private readonly Core.Logging.Logger log = LoggerFactory.GetLogger<LanServer>();
    private GameObject? gameObject;

    private readonly MultiplayerCommandRegistry commands;

    private WebSocketServer? network;
    private ConcurrentQueue<System.Action> commandQueue = new();

    public LanServer() : this(Dependencies.Get<MultiplayerCommandRegistry>()) { }

    public LanServer(MultiplayerCommandRegistry commands) {
        //log.Level = Core.Logging.LogLevel.Debug;
        instance = this;
        this.commands = commands;
    }

    public void Start() {
        LanConfiguration.reload();
        log.Info("Server preparing to listen on "+ LanConfiguration.instance.hostUrl);
        commandQueue = new();
        SetState(MultiplayerServerState.Preparing);
        network = new WebSocketServer(LanConfiguration.instance.serverPort);
        network.AddWebSocketService<LanServerClient>("/oni");
        SetState(MultiplayerServerState.Starting);
        network.Start();
        gameObject = UnityObject.CreateStaticWithComponent<ServerComponent>();
        SetState(MultiplayerServerState.Started);
    }

    public void Stop() {
        if (network == null) { return; }

        if (State <= MultiplayerServerState.Stopped)
            throw new NetworkPlatformException("Server isn't started");

        log.Info("Server preparing to stop");
        if (gameObject != null) {
            UnityObject.Destroy(gameObject);
        }
        network.Stop();
        network = null;
        SetState(MultiplayerServerState.Stopped);
    }

    public void Send(IMultiplayerClientId clientId, IMultiplayerCommand command) {
        var connections = new SingletonCollection<LanServerClient>(clientIdToLanClient[(LanMultiplayerClientId)clientId]);
        SendCommand(command, MultiplayerCommandOptions.None, connections);
    }

    public void Send(IMultiplayerCommand command, MultiplayerCommandOptions options = MultiplayerCommandOptions.None) {
        IEnumerable<KeyValuePair<LanMultiplayerClientId, LanServerClient>> recipients = clientIdToLanClient;
        if (options.HasFlag(MultiplayerCommandOptions.SkipHost) && LanClient.instance != null) {
            recipients = recipients.Where(entry => !entry.Key.Equals(LanClient.instance.Id));
        }

        SendCommand(command, options, recipients.Select(it => it.Value));
    }

    public void Tick() {
        while (commandQueue.TryDequeue(out var action)) {
            action();
        }
    }

    private void SetState(MultiplayerServerState state) {
        State = state;
        StateChanged?.Invoke(state);
    }

    private void SendCommand(IMultiplayerCommand command, MultiplayerCommandOptions options, IEnumerable<LanServerClient> connections) {
        var message = new NetworkMessage(command, options);
        SendMessage(message, connections);
    }

    private void SendMessage(NetworkMessage message, IEnumerable<LanServerClient> connections) {
        if (connections.Count() == 0) { return; }
        var clientId = lanClientIdToClientId[connections.First().ID];
        try {
            var data = message.toBytes();

            log.Debug("Server sending command " + message.Command.GetType() + " to " + connections.Count() + " clients (including " + clientId + "). Len " + data.Length);
            connections.ForEach(client => { client.SendData(data); });
        } catch (Exception e) {
            log.Warning("Server failed to send command " + message.Command.GetType() + " to " + connections.Count() + " clients (including " + clientId + "). " + e.Message);
        }
    }

    internal void AddClient(LanServerClient lanServerClient) {
        LanMultiplayerClientId clientId;
        if (lanClientIdToClientId.Count() == 0 && LanClient.instance != null) {
            clientId = LanClient.instance.lanMultiplayerClientId;
        } else {
            clientId = new LanMultiplayerClientId(lanServerClient.ID);
        }
        lanClientIdToClientId.Add(lanServerClient.ID, clientId);
        clientIdToLanClient.Add(clientId, lanServerClient);
        commandQueue.Enqueue(() => {
            log.Info("Server client connected - ID: " + clientId + ", IP: " + lanServerClient.Context.UserEndPoint.Address);
            ClientConnected?.Invoke(clientId);
        });
    }

    internal void RemoveClient(LanServerClient lanServerClient) {
        var clientId = lanClientIdToClientId[lanServerClient.ID];
        lanClientIdToClientId.Remove(lanServerClient.ID);
        clientIdToLanClient.Remove(clientId);
        commandQueue.Enqueue(() => {
            log.Info("Server client disconnected - ID: " + clientId);
            ClientDisconnected?.Invoke(clientId);
        });
    }

    internal void MessageReceived(LanServerClient lanServerClient, MessageEventArgs e) {
        var clientId = lanClientIdToClientId[lanServerClient.ID];
        var message = NetworkMessage.from(e.RawData);

        if (message.Command.GetType() != typeof(UpdatePlayerCursorPosition)) {
            log.Debug("Server received command " + message.Command.GetType() + " from client " + clientId.Id + ". Len " + e.RawData.Length);
        }

        var configuration = commands.GetCommandConfiguration(message.Command.GetType());
        commandQueue.Enqueue(() => {
            if (State != MultiplayerServerState.Starting
             && State != MultiplayerServerState.Started) {
                return;
            }

            if (configuration.ExecuteOnServer) {
                CommandReceived?.Invoke(clientId, message.Command);
            } else {
                var connections = clientIdToLanClient.Where(it => !it.Key.Equals(clientId)).Select(it => it.Value);
                SendMessage(message, connections);
            }
        });
    }

}

internal class LanServerClient : WebSocketBehavior {
    private readonly Core.Logging.Logger log = LoggerFactory.GetLogger<LanServerClient>();

    protected override void OnOpen() {
        LanServer.instance?.AddClient(this);
    }

    protected override void OnClose(CloseEventArgs e) {
        LanServer.instance?.RemoveClient(this);
    }

    protected override void OnMessage(MessageEventArgs e) {
        LanServer.instance?.MessageReceived(this, e);
    }

    protected override void OnError(ErrorEventArgs e) {
        log.Warning("Server client error " + e.Message);
    }

    internal void SendData(byte[] data) {
        Send(data);
    }
}

