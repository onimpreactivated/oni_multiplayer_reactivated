using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Changes priority for a certain action.
/// </summary>
/// <param name="target"></param>
/// <param name="priority"></param>
[Serializable]
public class ChangePriorityCommand(ComponentResolver<Prioritizable> target, PrioritySetting priority) : BaseCommandEvent
{
    /// <summary>
    /// The <see cref="Prioritizable"/> Resolver/Instance
    /// </summary>
    public ComponentResolver<Prioritizable> Target => target;

    /// <summary>
    /// Priority to set to.
    /// </summary>
    public PrioritySetting Priority => priority;
}
