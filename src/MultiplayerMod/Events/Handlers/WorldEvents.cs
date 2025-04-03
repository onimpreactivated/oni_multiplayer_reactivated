namespace MultiplayerMod.Events.Handlers;

public static class WorldEvents
{
    public static event OniEventHandler WorldLoading;
    public static event OniEventHandler WorldSync;
    public static event OniEventHandler WorldSyncRequested;
    public static event OniEventHandler WorldSaved;
    public static event OniEventHandler WorldStateInitializing;


    public static void OnWorldLoading()
    {
        WorldLoading.Invoke();
    }

    public static void OnWorldSync()
    {
        WorldSync.Invoke();
    }

    public static void OnWorldSyncRequested()
    {
        WorldSyncRequested.Invoke();
    }

    public static void OnWorldSaved()
    {
        WorldSaved.Invoke();
    }

    public static void OnWorldStateInitializing()
    {
        WorldStateInitializing.Invoke();
    }
}
