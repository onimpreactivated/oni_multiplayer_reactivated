using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Events.Others;
using MultiplayerMod.Multiplayer.UI.Overlays;

namespace MultiplayerMod.Commands;

internal class PlayerManagement
{
    internal static void ChangePlayerStateCommand_Event(ChangePlayerStateCommand changePlayerStateCommand)
    {
        var player = MultiplayerManager.Instance.MultiGame.Players[changePlayerStateCommand.PlayerId];
        player.State = changePlayerStateCommand.State;
        EventManager.TriggerEvent(new PlayerStateChangedEvent(player, changePlayerStateCommand.State));
        if (MultiplayerManager.Instance.MultiGame.Players.Ready)
            EventManager.TriggerEvent(new PlayersReadyEvent());
    }

    internal static void NotifyWorldSavePreparing_Event(NotifyWorldSavePreparingCommand _)
    {
        MultiplayerStatusOverlay.Show("Waiting for the world...");
    }

    internal static void LoadWorld_Event(LoadWorldCommand loadWorld)
    {
        MultiplayerManager.Instance.WorldManager.RequestWorldLoad(loadWorld.World);
    }

    internal static void SyncPlayersCommand_Event(SyncPlayersCommand syncPlayers)
    {
        MultiplayerManager.Instance.MultiGame.Players.Synchronize(syncPlayers.Players);
        EventManager.TriggerEvent(new PlayersUpdatedEvent(MultiplayerManager.Instance.MultiGame.Players));
    }

    internal static void RemovePlayerCommand_Event(RemovePlayerCommand playerCommand)
    {
        var player = MultiplayerManager.Instance.MultiGame.Players[playerCommand.PlayerId];
        MultiplayerManager.Instance.MultiGame.Players.Remove(playerCommand.PlayerId);
        EventManager.TriggerEvent(new PlayerLeftEvent(player, player.State == PlayerState.Leaving));
    }

    internal static void RequestPlayerStateChangeCommand_Event(RequestPlayerStateChangeCommand playerCommand)
    {
        MultiplayerManager.Instance.NetServer.Send(new ChangePlayerStateCommand(playerCommand.PlayerId, playerCommand.State));
    }

    internal static void RequestWorldSyncCommand_Event(RequestWorldSyncCommand _)
    {
        EventManager.TriggerEvent(new WorldSyncRequestedEvent());
    }

    internal static void InitializeClientCommand_Event(InitializeClientCommand initializeClient)
    {
        if (initializeClient.ClientId == null)
        {
            Debug.Log("Missing client id. Unable to initialize a player.");
            return;
        }
        Debug.Log("InitializeClientCommand_Event Create ClientInitializationRequestEvent!!!");
        EventManager.TriggerEvent(new ClientInitializationRequestEvent(initializeClient.ClientId, initializeClient.Profile));
    }

    internal static void AddPlayerCommand_Event(AddPlayerCommand addPlayerCommand)
    {
        MultiplayerManager.Instance.MultiGame.Players.Add(addPlayerCommand.Player);
        if (addPlayerCommand.Current)
        {
            MultiplayerManager.Instance.MultiGame.Players.SetCurrentPlayerId(addPlayerCommand.Player.Id);
            EventManager.TriggerEvent(new CurrentPlayerInitializedEvent(addPlayerCommand.Player));
        }
        EventManager.TriggerEvent(new PlayerJoinedEvent(addPlayerCommand.Player));
    }
}
