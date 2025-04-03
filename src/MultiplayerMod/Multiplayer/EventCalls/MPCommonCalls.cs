using MultiplayerMod.Core;
using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Events.Arguments.MultiplayerArg;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Multiplayer.UI.Overlays;
using UnityEngine;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class MPCommonCalls : BaseEventCall
{
    public override void Init()
    {
        MultiplayerEvents.JoinRequested += ConnectUserToEndPoint;
        MultiplayerEvents.MultiplayerStop += StopServer;
        GameEvents.GameStarted += TriggerBasicEvents;
        GameEvents.GameStarted += CreateMultiplayerGameObject;
        GameEvents.GameStarted += StartServer;
    }

    internal static void ConnectUserToEndPoint(JoinRequestedArg @event)
    {
        MainMenuEvents.OnMultiplayerModeSelected(Core.Player.PlayerRole.Client);
        MultiplayerStatusOverlay.Show($"Connecting to {@event.HostName}...");
        MultiplayerManager.Instance.NetClient.Connect(@event.Endpoint);
    }

    internal static void CreateMultiplayerGameObject()
    {
        Type[] components =
        [
            /*
#if DEBUG
            typeof(WorldDebugSnapshotRunner),
#endif
            */
            //typeof(PlayerCursor),
            typeof(MulitplayerNotifier)

        ];
        new GameObject("Multiplayer", components);
        Debug.Log("Game started!");
    }

    internal static void StartServer()
    {
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Server)
            return;
        MultiplayerStatusOverlay.Show("Starting host...");
        MultiplayerEvents.PlayersReady += CloseOverlayOnReady;
        MultiplayerManager.Instance.NetServer.Start();
    }

    internal static void StopServer()
    {
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Server)
            return;

        MultiplayerManager.Instance.NetServer.Stop();
    }

    internal static void TriggerBasicEvents()
    {
        GameEvents.OnGameReady();
        WorldEvents.OnWorldStateInitializing();
        MultiplayerEvents.OnMultiplayerStarted();
    }


    internal static void CloseOverlayOnReady()
    {
        MultiplayerStatusOverlay.Close();
        MultiplayerEvents.PlayersReady -= CloseOverlayOnReady;
    }
}
