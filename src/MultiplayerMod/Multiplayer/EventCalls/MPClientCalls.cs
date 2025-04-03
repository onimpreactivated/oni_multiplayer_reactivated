using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class MPClientCalls : BaseEventCall
{
    public override void Init()
    {
        MultiplayerManager.Instance.NetClient.StateChanged += OnClientStateChanged;
        MultiplayerEvents.MultiplayerStarted += StartClientWhenReady;
        MultiplayerEvents.MultiplayerStop += OnStopMultiplayer;
        GameEvents.GameQuit += OnGameQuit;
    }

    internal static void OnClientStateChanged(NetStateClient state)
    {
        if (state == NetStateClient.Connected)
        {
            // sending dlc check if we can play on same game.
            Debug.Log("Sending dlc check");
            MultiplayerManager.Instance.NetClient.Send(new DLC_CheckCommand(DlcManager.GetActiveDLCIds()), MultiplayerCommandOptions.OnlyHost);
        }
        if (state == NetStateClient.Error)
        {
            MultiplayerEvents.OnMultiplayerStop();
            MultiplayerEvents.OnConnectionLost();
        }
    }

    internal static void StartClientWhenReady()
    {
        if (MultiplayerManager.Instance.MultiGame.Mode != PlayerRole.Client)
            return;

        var currentPlayer = MultiplayerManager.Instance.MultiGame.Players.Current;
        MultiplayerManager.Instance.NetClient.Send(new RequestPlayerStateChangeCommand(currentPlayer.Id, PlayerState.Ready), MultiplayerCommandOptions.OnlyHost);
    }

    internal static void OnGameQuit()
    {
        MultiplayerManager.Instance.NetClient.Send(new RequestPlayerStateChangeCommand(MultiplayerManager.Instance.MultiGame.Players.Current.Id, PlayerState.Leaving), MultiplayerCommandOptions.OnlyHost);
        MultiplayerEvents.OnMultiplayerStop();
    }

    internal static void OnStopMultiplayer()
    {
        MultiplayerManager.Instance.NetClient.Disconnect();
        MultiplayerManager.Instance.MultiGame.Players.Synchronize([]);
    }
}
