using MultiplayerMod.Core.Logging;
using MultiplayerMod.Core.Scheduling;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.ModRuntime.StaticCompatibility;
using MultiplayerMod.Multiplayer.Commands;
using MultiplayerMod.Multiplayer.Commands.Player;
using MultiplayerMod.Multiplayer.UI.Overlays;
using MultiplayerMod.Network;
using MultiplayerMod.Platform.Common.Network.Components;
using MultiplayerMod.Platform.Common.Network.Messaging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

namespace MultiplayerMod.Platform.Lan.Network;

internal class LanClient : IMultiplayerClient {
    public static LanClient? instance = null;

    public IMultiplayerClientId Id => lanMultiplayerClientId;
    public MultiplayerClientState State { get; private set; } = MultiplayerClientState.Disconnected;

    public event Action<MultiplayerClientState>? StateChanged;
    public event Action<IMultiplayerCommand>? CommandReceived;

    internal readonly LanMultiplayerClientId lanMultiplayerClientId = new LanMultiplayerClientId();
    private WebSocket? network;

    private GameObject? gameObject = null!;

    private readonly Core.Logging.Logger log = LoggerFactory.GetLogger<LanClient>();
    private ConcurrentQueue<System.Action> commandQueue = new();

    public LanClient() {
        //log.Level = Core.Logging.LogLevel.Debug;
        instance = this;
    }

    public void Connect(IMultiplayerEndpoint endpoint) {
        LanConfiguration.reload();
        log.Info("Client connecting to server " + LanConfiguration.instance.hostUrl);
        commandQueue = new();
        network = new WebSocket(LanConfiguration.instance.hostUrl+"/oni");
        network.OnOpen += OnOpen;
        network.OnMessage += OnMessage;
        network.OnClose += OnClose;
        network.OnError += OnError;
        SetState(MultiplayerClientState.Connecting);
        network.ConnectAsync();
        gameObject = UnityObject.CreateStaticWithComponent<ClientComponent>();
    }

    public void Disconnect() {
        if (network == null) { return; }
        log.Info("Client disconnected from server");
        var oldnetwork = network;
        network = null;
        oldnetwork.CloseAsync();
        if (gameObject != null) {
            UnityObject.Destroy(gameObject);
            gameObject = null!;
        }
    }

    public void Send(IMultiplayerCommand command, MultiplayerCommandOptions options = MultiplayerCommandOptions.None) {
        if (network == null) { return; }
        try {
            var data = new NetworkMessage(command, options).toBytes();

            if (command.GetType() != typeof(UpdatePlayerCursorPosition)) {
                log.Debug("Client sending command " + command.GetType() + ". Client " + Id + ". Len " + data.Length);
            }
            network.Send(data);
        } catch (Exception e) {
            log.Debug("Client failed to send command " + command.GetType() + ". " + e.Message);
        }
    }

    public void Tick() {
        while (commandQueue.TryDequeue(out var action)) {
            action();
        }
    }

    private void SetState(MultiplayerClientState status) {
        State = status;
        StateChanged?.Invoke(status);
    }

    private void OnOpen(object sender, EventArgs e) {
        log.Debug("Client connected to server");
        commandQueue.Enqueue(() => {
            SetState(MultiplayerClientState.Connected);
        });
    }

    private void OnClose(object sender, CloseEventArgs e) {
        log.Debug("Client disconnected from server");
        commandQueue.Enqueue(() => {
            if (State == MultiplayerClientState.Connecting) {
                log.Warning("Multiplayer connection to server could not be established.");
                MultiplayerStatusOverlay.Text = "Failed to connect to server";
                Task _ = this.closeOverlay();
            }
            SetState(MultiplayerClientState.Disconnected);
            Disconnect();
        });
    }

    private async Task closeOverlay() {
        await Task.Delay(2000);
        Dependencies.Get<UnityTaskScheduler>().Run(MultiplayerStatusOverlay.Close);
    }

    private void OnMessage(object sender, MessageEventArgs e) {
        var message = NetworkMessage.from(e.RawData);
        log.Debug("Client received command " + message.Command.GetType() + ". Client " + Id + ". Len " + e.RawData.Length);
        commandQueue.Enqueue(() => {
            if (State != MultiplayerClientState.Connected) { return; }
            CommandReceived?.Invoke(message.Command);
        });
    }

    private void OnError(object sender, ErrorEventArgs e) {
        log.Warning("Client error " + e.Message);
        commandQueue.Enqueue(() => {
            SetState(MultiplayerClientState.Error);
        });
    }

}
