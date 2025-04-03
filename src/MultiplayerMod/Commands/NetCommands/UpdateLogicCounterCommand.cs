using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Update <see cref="LogicCounter"/>
/// </summary>
/// <param name="target"></param>
/// <param name="currentCount"></param>
/// <param name="maxCount"></param>
/// <param name="advancedMode"></param>
[Serializable]
public class UpdateLogicCounterCommand(
    ComponentResolver<LogicCounter> target,
    int currentCount,
    int maxCount,
    bool advancedMode) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="LogicCounter"/>
    /// </summary>
    public ComponentResolver<LogicCounter> Target => target;

    /// <summary>
    /// Current Count
    /// </summary>
    public int CurrentCount => currentCount;

    /// <summary>
    /// Max Count
    /// </summary>
    public int MaxCount => maxCount;

    /// <summary>
    /// Is Advanced Mode
    /// </summary>
    public bool AdvancedMode => advancedMode;
}
