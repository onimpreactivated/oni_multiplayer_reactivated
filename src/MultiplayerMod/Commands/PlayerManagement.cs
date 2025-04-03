using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Multiplayer.UI.Overlays;

namespace MultiplayerMod.Commands;

internal class PlayerManagement
{
    internal static void ChangePlayerStateCommand_Event(ChangePlayerStateCommand changePlayerStateCommand)
    {
        var player = MultiplayerManager.Instance.MultiGame.Players[changePlayerStateCommand.PlayerId];
        player.State = changePlayerStateCommand.State;
        PlayerEvents.OnPlayerStateChanged(player, changePlayerStateCommand.State);
        if (MultiplayerManager.Instance.MultiGame.Players.Ready)
            MultiplayerEvents.OnPlayersReady();
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
        PlayerEvents.OnPlayersUpdated(MultiplayerManager.Instance.MultiGame.Players);
    }

    internal static void RemovePlayerCommand_Event(RemovePlayerCommand playerCommand)
    {
        var player = MultiplayerManager.Instance.MultiGame.Players[playerCommand.PlayerId];
        MultiplayerManager.Instance.MultiGame.Players.Remove(playerCommand.PlayerId);
        PlayerEvents.OnPlayerLeft(player, player.State == PlayerState.Leaving);
    }

    internal static void RequestPlayerStateChangeCommand_Event(RequestPlayerStateChangeCommand playerCommand)
    {
        MultiplayerManager.Instance.NetServer.Send(new ChangePlayerStateCommand(playerCommand.PlayerId, playerCommand.State));
    }

    internal static void RequestWorldSyncCommand_Event(RequestWorldSyncCommand _)
    {
        WorldEvents.OnWorldSyncRequested();
    }

    internal static void InitializeClientCommand_Event(InitializeClientCommand initializeClient)
    {
        if (initializeClient.ClientId == null)
        {
            Debug.Log("Missing client id. Unable to initialize a player.");
            return;
        }
        MultiplayerEvents.OnClientInitializationRequest(initializeClient.ClientId, initializeClient.Profile);
    }

    internal static void AddPlayerCommand_Event(AddPlayerCommand addPlayerCommand)
    {
        MultiplayerManager.Instance.MultiGame.Players.Add(addPlayerCommand.Player);
        if (addPlayerCommand.Current)
        {
            MultiplayerManager.Instance.MultiGame.Players.SetCurrentPlayerId(addPlayerCommand.Player.Id);
            PlayerEvents.OnCurrentPlayerInitialized(addPlayerCommand.Player);
        }
        PlayerEvents.OnPlayerJoined(addPlayerCommand.Player);
    }
}
