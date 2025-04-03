namespace MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;

internal class SafeCellMonitorSync : BaseChoreSync<SafeCellMonitor>
{
    public override Type SyncType => typeof(SafeCellMonitor);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        SM.danger.enterActions.Clear();
    }

    public override void Server(StateMachine instance)
    {

    }
}
