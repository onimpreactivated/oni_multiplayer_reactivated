namespace MultiplayerMod.Events.Arguments.Chores;

public class ChoreCleanupEventArg : EventArgs
{
    public Chore Chore { get; }

    public ChoreCleanupEventArg(Chore chore)
    {
        Chore = chore;
    }
}
