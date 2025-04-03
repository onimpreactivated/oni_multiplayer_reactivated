using MultiplayerMod.Core.Objects;

namespace MultiplayerMod.Events.Chores;

/// <summary>
/// Simple event that called when new Chore created
/// </summary>
/// <param name="chore"></param>
/// <param name="id"></param>
/// <param name="type"></param>
/// <param name="arguments"></param>
public class ChoreCreatedEvent(Chore chore, MultiplayerId id, Type type, object[] arguments) : BaseEvent
{
    /// <summary>
    /// The created Chore
    /// </summary>
    public Chore Chore => chore;
    /// <summary>
    /// Id of the <see cref="Chore"/>
    /// </summary>
    public MultiplayerId Id => id;

    /// <summary>
    /// Type of the created Chore
    /// </summary>
    public Type Type = type;

    /// <summary>
    /// Argument used to create
    /// </summary>
    public object[] Arguments => arguments;
}
