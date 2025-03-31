using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class IdleStateSync : BaseChoreSync<IdleStates>
{
    public override Type SyncType => typeof(IdleStates);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        SM.loop.enterActions.Clear();

        var targetCell = AddMultiplayerParameter<int, IdleStates.IntParameter>(MoveObjectToCellCommand.TargetCell);
        SM.move.Enter(smi => {
            var navigator = smi.GetComponent<Navigator>();
            navigator.GoTo(targetCell.Get(smi));
        });
    }

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        SM.move.Enter(smi => {
            var target = smi.GetComponent<Navigator>().targetLocator;
            var cell = Grid.PosToCell(target);

            // TODO: Remove after critters sync (WorldGenSpawner.Spawnable + new critters)
            if (MultiplayerManager.Instance.MPObjects.Get(smi.master.gameObject) == null)
                return;

            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new StateMachineResolver(smi.controller.GetComponentResolver(), smi.GetType()), cell, SM.move),
                MultiplayerCommandOptions.SkipHost
            );
        });
        SM.move.Exit(smi => {

            // TODO: Remove after critters sync (WorldGenSpawner.Spawnable + new critters)
            if (MultiplayerManager.Instance.MPObjects.Get(smi.master.gameObject) == null)
                return;

            MultiplayerManager.Instance.NetServer.Send(
                new GoToStateCommand(new StateMachineResolver(smi.controller.GetComponentResolver(), smi.GetType()), SM.loop),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }
}
