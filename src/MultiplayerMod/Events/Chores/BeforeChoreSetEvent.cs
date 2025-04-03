namespace MultiplayerMod.Events.Chores;

/// <summary>
/// Called before the <see cref="Chore"/> set
/// </summary>
public class BeforeChoreSetEvent : BaseEvent
{
    /// <summary>
    /// The <see cref="ChoreDriver"/>
    /// </summary>
    public ChoreDriver Driver { get; internal set; }

    /// <summary>
    /// The previous <see cref="Chore"/>
    /// </summary>
    public Chore PreviousChore { get; internal set; }

    /// <summary>
    /// The <see cref="Chore.Precondition.Context"/>
    /// </summary>
    public Chore.Precondition.Context Context { get; internal set; }

    /// <summary>
    /// Called before the <see cref="Chore"/> set
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="previousChore"></param>
    /// <param name="context"></param>
    public BeforeChoreSetEvent(ChoreDriver driver, Chore previousChore, ref Chore.Precondition.Context context)
    {
        Driver = driver;
        PreviousChore = previousChore;
        Context = context;
    }
}
