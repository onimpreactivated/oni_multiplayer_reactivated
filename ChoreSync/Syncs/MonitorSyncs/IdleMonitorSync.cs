using MultiplayerMod.Core.Wrappers;

namespace MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;

internal class IdleMonitorSync : BaseChoreSync<IdleMonitor>
{
    public override Type SyncType => typeof(IdleMonitor);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        SM.idle.enterActions.Clear();
    }

    public override void Server(StateMachine instance)
    {

    }
}
