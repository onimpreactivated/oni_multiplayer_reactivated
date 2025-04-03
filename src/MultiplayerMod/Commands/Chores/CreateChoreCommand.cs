using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Wrappers;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Called when a <see cref="Chore"/> has been created.
/// </summary>
[Serializable]
public class CreateChoreCommand(MultiplayerId id, Type choreType, object[] arguments) : BaseCommandEvent
{
    /// <summary>
    /// The <see cref="MultiplayerId"/> of <see cref="Chore"/>
    /// </summary>
    public MultiplayerId MultiId => id;

    /// <summary>
    /// The <see cref="Type"/> of the <see cref="Chore"/>
    /// </summary>
    public Type ChoreType => choreType;

    /// <summary>
    /// Argument that the <see cref="Chore"/> created
    /// </summary>
    public object[] Arguments => arguments;
}
