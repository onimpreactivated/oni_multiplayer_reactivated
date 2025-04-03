using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Update <see cref="LogicTimeOfDaySensor"/>
/// </summary>
/// <param name="target"></param>
/// <param name="startTime"></param>
/// <param name="duration"></param>
[Serializable]
public class UpdateLogicTimeOfDaySensorCommand(
    ComponentResolver<LogicTimeOfDaySensor> target,
    float startTime,
    float duration): BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="LogicTimeOfDaySensor"/>
    /// </summary>
    public ComponentResolver<LogicTimeOfDaySensor> Target => target;

    /// <summary>
    /// The <see cref="LogicTimeOfDaySensor.startTime"/>
    /// </summary>
    public float StartTime => startTime;

    /// <summary>
    /// The <see cref="LogicTimeOfDaySensor.duration"/>
    /// </summary>
    public float Duration => duration;
}
