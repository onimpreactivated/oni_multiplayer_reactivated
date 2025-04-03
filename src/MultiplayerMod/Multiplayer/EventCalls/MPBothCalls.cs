using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Arguments.CorePlayerArgs;
using MultiplayerMod.Events.Handlers;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class MPBothCalls : BaseEventCall
{
    public override void Init()
    {
        PlayerEvents.CurrentPlayerInitialized += OnCurrentPlayerInitialized;
    }

    internal static void OnCurrentPlayerInitialized(CorePlayerArg @event)
    {
        MultiplayerManager.Instance.NetClient.Send(new RequestPlayerStateChangeCommand(@event.CorePlayer.Id, PlayerState.Loading), Network.Common.MultiplayerCommandOptions.OnlyHost);
        if (@event.CorePlayer.Role == PlayerRole.Server)
            MultiplayerManager.Instance.NetServer.Send(new ChangePlayerStateCommand(@event.CorePlayer.Id, PlayerState.Ready));
        else
            MultiplayerManager.Instance.NetClient.Send(new RequestWorldSyncCommand(), Network.Common.MultiplayerCommandOptions.OnlyHost);
    }
}
