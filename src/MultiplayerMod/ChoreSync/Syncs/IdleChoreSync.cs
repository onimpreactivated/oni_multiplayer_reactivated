using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class IdleChoreSync : BaseChoreSync<IdleChore.States>
{
    public override Type SyncType => typeof(IdleChore);

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        SM.idle.move.Enter(smi =>
        {
            Debug.Log("(IdleChoreSync) Sever idle.move Enter!");
            var cell = smi.GetIdleCell();
            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new ChoreStateMachineResolver(smi.master), cell, SM.idle.move),
                MultiplayerCommandOptions.SkipHost
            );
        });
        SM.idle.move.Exit(smi =>
        {
            Debug.Log("(IdleChoreSync) Sever idle.move Exit!");
            MultiplayerManager.Instance.NetServer.Send(
                new GoToStateCommand(new ChoreStateMachineResolver(smi.master), SM.idle),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }

    public override void Client(StateMachine instance)
    {
        Setup(instance);

        /*
        SM.idle.onfloor.ToggleScheduleCallback("", null, null);
        SM.idle.onladder.ToggleScheduleCallback("", null, null);
        SM.idle.ontube.Update("", null, 0, false);
        SM.idle.onsuitmarker.ToggleScheduleCallback("", null, null);
        SM.idle.move.Transition(null, null, 0);
        SM.idle.move.MoveTo(null, null, null, false);
        */
        var targetCell = AddMultiplayerParameter<int, IdleChore.States.IntParameter>(MoveObjectToCellCommand.TargetCell);
        SM.idle.move.MoveTo(targetCell.Get, SM.idle, SM.idle);
    }
}
