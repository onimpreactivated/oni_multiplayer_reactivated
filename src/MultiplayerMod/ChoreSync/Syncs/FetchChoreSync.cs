using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class FetchAreaChoreSync : BaseChoreSync<FetchAreaChore.States>
{
    public override Type SyncType => typeof(FetchAreaChore);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        SM.root.enterActions.Clear();
        SM.root.enterActions.Add(new("Transit to Empty State", (FetchAreaChore.StatesInstance smi) =>
        {
            Debug.Log("(FetchAreaChoreSync) Client root.enterActions.");
            smi.GoTo((StateMachine.BaseState) null);
        }));
        SM.delivering.next.enterActions.Clear();
        SM.delivering.next.enterActions.Add(new("Transit to Empty State", (FetchAreaChore.StatesInstance smi) =>
        {
            Debug.Log("(FetchAreaChoreSync) Client delivering.next.enterActions.");
            smi.GoTo((StateMachine.BaseState) null);
        }));
        SM.fetching.next.enterActions.Clear();
        SM.fetching.next.enterActions.Add(new("Transit to Empty State", (FetchAreaChore.StatesInstance smi) =>
        {
            Debug.Log("(FetchAreaChoreSync) Client fetching.next.enterActions.");
            smi.GoTo((StateMachine.BaseState) null);
        }));
    }

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        SM.delivering.next.Exit("Trigger Multiplayer move event", (smi) =>
        {
            Debug.Log("(FetchAreaChoreSync) Server exit delivering.");
            var stack = smi.gotoStack;
            Debug.Log($"Stack: {string.Join(", ", stack)}");
            var dest = SM.deliveryDestination.Get(smi);
            Debug.Log($"dest: {dest}");
            var obj = SM.deliveryObject.Get(smi);
            Debug.Log($"dest: {obj}");

            if (!MultiplayerManager.Instance.MultiGame.Players.Ready)
                return;
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
        SM.fetching.next.Exit("Trigger Multiplayer move event", (smi) =>
        {
            Debug.Log("(FetchAreaChoreSync) Server exit fetching.");
            var stack = smi.gotoStack;
            Debug.Log($"Stack: {string.Join(", ", stack)}");
            var fetchTarget = SM.fetchTarget.Get(smi);
            Debug.Log($"fetchTarget: {fetchTarget}");
            var fetchResultTarget = SM.fetchResultTarget.Get(smi);
            Debug.Log($"fetchResultTarget: {fetchResultTarget}");
            var fetchAmount = SM.fetchAmount.Get(smi);
            Debug.Log($"fetchAmount: {fetchAmount}");

            if (!MultiplayerManager.Instance.MultiGame.Players.Ready)
                return;

            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }
}
