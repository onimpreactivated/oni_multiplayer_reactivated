using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Common;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class MPBothCalls
{
    internal static void OnCurrentPlayerInitialized(CurrentPlayerInitializedEvent @event)
    {
        MultiplayerManager.Instance.NetClient.Send(new RequestPlayerStateChangeCommand(@event.Player.Id, PlayerState.Loading), Network.Common.MultiplayerCommandOptions.OnlyHost);
        if (@event.Player.Role == PlayerRole.Server)
            MultiplayerManager.Instance.NetServer.Send(new ChangePlayerStateCommand(@event.Player.Id, PlayerState.Ready));
        else
            MultiplayerManager.Instance.NetClient.Send(new RequestWorldSyncCommand(), Network.Common.MultiplayerCommandOptions.OnlyHost);
    }
}
