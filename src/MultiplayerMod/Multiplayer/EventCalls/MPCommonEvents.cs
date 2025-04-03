using MultiplayerMod.Core;
using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Events.MainMenu;
using MultiplayerMod.Events.Others;
using MultiplayerMod.Multiplayer.UI.Overlays;
using UnityEngine;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class MPCommonEvents
{
    internal static void ConnectUserToEndPoint(MultiplayerJoinRequestedEvent @event)
    {
        EventManager.TriggerEvent(new MultiplayerModeSelectedEvent(Core.Player.PlayerRole.Client));
        MultiplayerStatusOverlay.Show($"Connecting to {@event.HostName}...");
        MultiplayerManager.Instance.NetClient.Connect(@event.Endpoint);
    }

    internal static void CreateMultiplayerGameObject(GameStartedEvent _)
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

    internal static void StartServer(GameStartedEvent _)
    {
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Server)
            return;
        MultiplayerStatusOverlay.Show("Starting host...");
        EventManager.SubscribeEvent<PlayersReadyEvent>(CloseOverlayOnReady);
        MultiplayerManager.Instance.NetServer.Start();
    }

    internal static void StopServer(StopMultiplayerEvent _)
    {
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Server)
            return;

        MultiplayerManager.Instance.NetServer.Stop();
    }

    internal static void TriggerBasicEvents(GameStartedNoArgsEvent _)
    {
        EventManager.TriggerEvent(new GameReadyEvent());
        EventManager.TriggerEvent(new WorldStateInitializingEvent());
        EventManager.TriggerEvent(new GameStartedEvent());
    }


    [NoAutoSubscribe]
    [UnsubAfterCall]
    internal static void CloseOverlayOnReady(PlayersReadyEvent _)
    {
        MultiplayerStatusOverlay.Close();
    }
}
