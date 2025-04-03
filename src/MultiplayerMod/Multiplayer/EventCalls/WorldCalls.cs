using MultiplayerMod.Core;
using MultiplayerMod.Events.Common;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class WorldCalls
{
    internal static void WorldSyncRequestedEvent_Event(WorldSyncRequestedEvent _)
    {
        MultiplayerManager.Instance.WorldManager.Sync();
    }

    internal static void WorldSyncRequestedEvent_Event(WorldSavedEvent _)
    {
        MultiplayerManager.Instance.WorldManager.Sync();
    }
}
