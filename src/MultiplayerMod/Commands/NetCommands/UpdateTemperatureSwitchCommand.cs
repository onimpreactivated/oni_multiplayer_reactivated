using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Updating the <see cref="TemperatureControlledSwitch"/>
/// </summary>
/// <param name="target"></param>
/// <param name="thresholdTemperature"></param>
/// <param name="activateOnWarmerThan"></param>
[Serializable]
public class UpdateTemperatureSwitchCommand(
    ComponentResolver<TemperatureControlledSwitch> target,
    float thresholdTemperature,
    bool activateOnWarmerThan) : BaseCommandEvent

{
    /// <summary>
    /// Resolver for <see cref="TemperatureControlledSwitch"/>
    /// </summary>
    public ComponentResolver<TemperatureControlledSwitch> Target => target;

    /// <summary>
    /// The threshold temperature
    /// </summary>
    public float ThresholdTemperature => thresholdTemperature;

    /// <summary>
    /// Should activate warmer than that.
    /// </summary>
    public bool ActivateOnWarmerThan => activateOnWarmerThan;
}
