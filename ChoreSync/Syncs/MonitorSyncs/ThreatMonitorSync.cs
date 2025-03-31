namespace MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;

internal class ThreatMonitorSync : BaseChoreSync<ThreatMonitor>
{
    public override Type SyncType => typeof(ThreatMonitor);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        SM.threatened.duplicant.ShouldFight.enterActions.Clear();
    }

    public override void Server(StateMachine instance)
    {

    }
}
