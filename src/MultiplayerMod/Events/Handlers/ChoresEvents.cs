using MultiplayerMod.Core.Objects;
using MultiplayerMod.Events.Arguments.Chores;

namespace MultiplayerMod.Events.Handlers;

public static class ChoresEvents
{
    public static event OniEventHandlerTEventArgs<ChoreCreatedEvent> ChoreCreated;
    public static event OniEventHandlerTEventArgs<ChoreCleanupEventArg> ChoreCleanup;
    public static event OniEventHandlerTEventArgs<ChoreBeforeSetEventArg> ChoreBeforeSetEvent;

    public static void OnChoreCreated(StandardChoreBase chore, MultiplayerId id, Type type, object[] arguments)
    {
        ChoreCreated?.Invoke(new(chore, id, type, arguments));
    }

    public static void OnChoreCleanup(Chore chore)
    {
        ChoreCleanup?.Invoke(new(chore));
    }

    public static void OnChoreBeforeSetEvent(ChoreDriver driver, Chore previousChore, ref Chore.Precondition.Context context)
    {
        ChoreBeforeSetEvent?.Invoke(new(driver, previousChore, ref context));
    }
}
