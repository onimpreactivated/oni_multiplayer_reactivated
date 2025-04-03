using MultiplayerMod.Core;
using MultiplayerMod.Events.Handlers;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class WorldCalls : BaseEventCall
{
    public override void Init()
    {
        WorldEvents.WorldSyncRequested += WorldSyncRequestedEvent_Event;
        WorldEvents.WorldSaved += WorldSyncRequestedEvent_Event;
    }
    internal static void WorldSyncRequestedEvent_Event()
    {
        MultiplayerManager.Instance.WorldManager.Sync();
    }
}
