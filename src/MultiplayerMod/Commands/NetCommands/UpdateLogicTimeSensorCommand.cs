using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Update for <see cref="LogicTimerSensor"/>
/// </summary>
/// <param name="target"></param>
/// <param name="displayCyclesMode"></param>
/// <param name="onDuration"></param>
/// <param name="offDuration"></param>
/// <param name="timeElapsedInCurrentState"></param>
[Serializable]
public class UpdateLogicTimeSensorCommand(ComponentResolver<LogicTimerSensor> target,
    bool displayCyclesMode,
    float onDuration,
    float offDuration,
    float timeElapsedInCurrentState) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="LogicTimerSensor"/>
    /// </summary>
    public ComponentResolver<LogicTimerSensor> Target => target;

    /// <summary>
    /// The <see cref="LogicTimerSensor.displayCyclesMode"/>
    /// </summary>
    public bool DisplayCyclesMode => displayCyclesMode;

    /// <summary>
    /// The <see cref="LogicTimerSensor.onDuration"/>
    /// </summary>
    public float OnDuration => onDuration;

    /// <summary>
    /// The <see cref="LogicTimerSensor.offDuration"/>
    /// </summary>
    public float OffDuration => offDuration;

    /// <summary>
    /// The <see cref="LogicTimerSensor.timeElapsedInCurrentState"/>
    /// </summary>
    public float TimeElapsedInCurrentState => timeElapsedInCurrentState;
}
