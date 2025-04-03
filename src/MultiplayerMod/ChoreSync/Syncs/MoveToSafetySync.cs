using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class MoveToSafetySync : BaseChoreSync<MoveToSafetyChore.States>
{
    public override Type SyncType => typeof(MoveToSafetyChore);

    readonly StateInfo movingStateInfo = new (name: "__moving");
    public override void Client(StateMachine instance)
    {
        Setup(instance);
        var waiting = AddState<MoveToSafetyChore.States.State, MoveToSafetyChore.States.State>(SM.root, "__waiting" , this.SM.BindState);
        var moving = AddState<MoveToSafetyChore.States.State, MoveToSafetyChore.States.State>(SM.root, movingStateInfo, this.SM.BindState);

        var targetCell = AddMultiplayerParameter<int, MoveToSafetyChore.States.IntParameter>(MoveObjectToCellCommand.TargetCell);

        moving.MoveTo(targetCell.Get, waiting, waiting, true);

        SM.root.Enter(smi => smi.GoTo(waiting));
    }

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        SM.move.Enter(smi => {
            Debug.Log("(MoveToSafetySync) Sever move Enter!");
            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new ChoreStateMachineResolver(smi.master), smi.targetCell, movingStateInfo),
                MultiplayerCommandOptions.SkipHost
            );
        });

        SM.move.Update((smi, _) => {
            Debug.Log("(MoveToSafetySync) Sever move Update!");
            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new ChoreStateMachineResolver(smi.master), smi.targetCell, movingStateInfo),
                MultiplayerCommandOptions.SkipHost
            );
        });

        SM.move.Exit(smi => {
            Debug.Log("(MoveToSafetySync) Sever move Exit!");
            MultiplayerManager.Instance.NetServer.Send(
                new GoToStateCommand(new ChoreStateMachineResolver(smi.master), (StateMachine.BaseState) null),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }
}
