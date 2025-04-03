using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Events.Chores;

namespace MultiplayerMod.Multiplayer.Chores;

internal static class BindChoreSend
{
    internal static void MultiplayerChoreCreatedEvent_Call(ChoreCreatedEvent @event)
    {
        Debug.Log($"ChoreCreatedEvent: Type: {@event.Chore.GetType()} Arg: {string.Join(", ", @event.Arguments)}");
        var new_args = ArgumentUtils.WrapObjects(ChoreArgumentsWrapper.Wrap(@event.Type, @event.Arguments));
        var command = new CreateChoreCommand(@event.Id, @event.Type, new_args);
        Debug.Log($"CreateChoreCommand: Type: {command.ChoreType} Arg: {string.Join(", ", command.Arguments)}");
        MultiplayerManager.Instance.NetServer.Send(command, Network.Common.MultiplayerCommandOptions.SkipHost);
    }
}
