using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Wrappers;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Called when a <see cref="Chore"/> has been created.
/// </summary>
[Serializable]
public class CreateChoreCommand : BaseCommandEvent
{
    /// <summary>
    /// The <see cref="MultiplayerId"/> of <see cref="Chore"/>
    /// </summary>
    public MultiplayerId MultiId { get; }

    /// <summary>
    /// The <see cref="Type"/> of the <see cref="Chore"/>
    /// </summary>
    public Type ChoreType { get; }

    /// <summary>
    /// Argument that the <see cref="Chore"/> created
    /// </summary>
    public object[] Arguments { get; }

    public CreateChoreCommand(MultiplayerId id, Type choreType, object[] arguments)
    {
        MultiId = id;
        ChoreType = choreType;
        Arguments = ArgumentUtils.WrapObjects(ChoreArgumentsWrapper.Wrap(ChoreType, arguments));
    }
}
