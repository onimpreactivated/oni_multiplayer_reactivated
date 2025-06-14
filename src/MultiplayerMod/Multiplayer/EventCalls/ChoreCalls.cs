using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Multiplayer.Controllers;
using MultiplayerMod.Network.Common;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class ChoreCalls : BaseEventCall
{
    public override void Init()
    {
        ChoresEvents.ChoreCreated += ChoresEvents_ChoreCreated;
        ChoresEvents.ChoreBeforeSetEvent += ChoresEvents_ChoreBeforeSetEvent;
    }
    private static readonly ConditionalWeakTable<ChoreDriver, BoxedValue<bool>> driverSynchronizationState = new();

    private void ChoresEvents_ChoreBeforeSetEvent(Events.Arguments.Chores.ChoreBeforeSetEventArg ev)
    {
        var synchronized = driverSynchronizationState.GetValue(ev.Driver, _ => new BoxedValue<bool>(false));
        var shouldReleaseDriver = synchronized.Value && ev.PreviousChore != null && ChoresController.Supported(ev.PreviousChore);
        if (shouldReleaseDriver)
        {
            MultiplayerManager.Instance.NetServer.Send(new ReleaseChoreDriverCommand(ev.Driver), MultiplayerCommandOptions.SkipHost);
            synchronized.Value = false;
        }

        if (!ChoresController.Supported(ev.Context.chore))
            return;

        var command = new SetDriverChoreCommand(ev.Driver, ev.Context.consumerState.consumer, ev.Context.chore, ev.Context.data);
        MultiplayerManager.Instance.NetServer.Send(command, MultiplayerCommandOptions.SkipHost);
        synchronized.Value = true;
    }

    private void ChoresEvents_ChoreCreated(Events.Arguments.Chores.ChoreCreatedEvent ev)
    {
        MultiplayerManager.Instance.NetServer.Send(new CreateChoreCommand(ev.Id, ev.Type, ev.Arguments), MultiplayerCommandOptions.SkipHost);
    }
}
