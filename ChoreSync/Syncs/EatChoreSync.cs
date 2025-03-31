using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class EatChoreSync : BaseChoreSync<EatChore.States>
{
    public override Type SyncType => typeof(EatChore);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        SM.root.enterActions.Clear();
        SM.root.enterActions.Add(new("Transit to Empty State", (EatChore.StatesInstance smi) =>
        {
            Debug.Log("(EatChoreSync) Client root.enterActions.");
            smi.GoTo((StateMachine.BaseState) null);
        }));
        SM.eatonfloorstate.enterActions.Clear();
        SM.eatonfloorstate.enterActions.Add(new("Transit to Empty State", (EatChore.StatesInstance smi) =>
        {
            Debug.Log("(EatChoreSync) Client eatonfloorstate.enterActions.");
            smi.GoTo((StateMachine.BaseState) null);
        }));
    }

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        SM.root.Enter("Trigger Multiplayer Enter event", (smi) =>
        {
            Debug.Log("(EatChoreSync) Server enter root.");

            if (!MultiplayerManager.Instance.MultiGame.Players.Ready)
                return;
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SetParameterValueCommand(smi, SM.messstation, SM.messstation.Get(smi)),
                MultiplayerCommandOptions.SkipHost
            );
        });
        SM.eatonfloorstate.Enter("Trigger Multiplayer Enter event", (smi) =>
        {
            Debug.Log("(EatChoreSync) Server enter eatonfloorstate.");
            var stack = smi.gotoStack;
            Debug.Log($"Stack: {string.Join(", ", stack)}");

            if (!MultiplayerManager.Instance.MultiGame.Players.Ready)
                return;

            MultiplayerManager.Instance.NetServer.Send(
                new SetParameterValueCommand(smi, SM.locator, SM.locator.Get(smi)),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }
}
