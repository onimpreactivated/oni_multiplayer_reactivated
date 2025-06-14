using MultiplayerMod.Core.Objects;

namespace MultiplayerMod.Events.Arguments.Chores;

public class ChoreCreatedEvent : EventArgs
{
    public StandardChoreBase Chore { get; }
    public MultiplayerId Id { get; }
    public Type Type { get; }
    public object[] Arguments { get; }

    public ChoreCreatedEvent(StandardChoreBase chore, MultiplayerId id, Type type, object[] arguments)
    {
        this.Chore = chore;
        this.Id = id;
        this.Type = type;
        this.Arguments = arguments;
    }
}

