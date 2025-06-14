
namespace MultiplayerMod.Events.Arguments.Chores;

public class ChoreBeforeSetEventArg : EventArgs
{
    public ChoreDriver Driver { get; }
    /// <summary>
    /// This can be null!
    /// </summary>
    public Chore PreviousChore { get; }
    private Chore.Precondition.Context _context;
    public ref Chore.Precondition.Context Context => ref _context;

    public ChoreBeforeSetEventArg(ChoreDriver driver, Chore previousChore, ref Chore.Precondition.Context context)
    {
        Driver = driver;
        PreviousChore = previousChore;
        _context = context;
    }
}
