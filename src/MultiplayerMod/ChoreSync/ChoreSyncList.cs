using MultiplayerMod.ChoreSync.Syncs;
using MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;

namespace MultiplayerMod.ChoreSync;

internal static class ChoreSyncList
{
    public static List<IChoreSync> Chores = [];

    static ChoreSyncList()
    {
        Register(new IdleChoreSync());
        Register(new IdleStateSync());
        Register(new MoveToSafetySync());
        Register(new FetchAreaChoreSync());
        Register(new EatChoreSync());
        // Monitor Syncs
        Register(new IdleMonitorSync());
        Register(new SafeCellMonitorSync());
    }

    public static void Register(IChoreSync chore)
    {
        Chores.Add(chore);
    }

    public static IChoreSync GetSync(Type ChoreSyncType)
    {
        return Chores.FirstOrDefault(chore => chore.SyncType == ChoreSyncType);
    }

    public static List<Type> GetStateMachineTypes()
    {
        return Chores.Select(chore => chore.StateMachineType).ToList();
    }

    public static List<Type> GetSyncTypes()
    {
        return Chores.Select(chore => chore.SyncType).ToList();
    }
}
