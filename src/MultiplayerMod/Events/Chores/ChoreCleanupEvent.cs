namespace MultiplayerMod.Events.Chores;

/// <summary>
/// Event called when <see cref="Chore.Cleanup"/> called
/// </summary>
/// <param name="chore"></param>
public class ChoreCleanupEvent(Chore chore) : BaseEvent
{
    /// <summary>
    /// The cores that called.
    /// </summary>
    public Chore Chore => chore;
}
