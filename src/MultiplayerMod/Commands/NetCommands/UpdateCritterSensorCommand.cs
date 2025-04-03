using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Update <see cref="LogicCritterCountSensor"/>
/// </summary>
/// <param name="target"></param>
/// <param name="countCritters"></param>
/// <param name="countEggs"></param>
[Serializable]
public class UpdateCritterSensorCommand(
        ComponentResolver<LogicCritterCountSensor> target,
        bool countCritters,
        bool countEggs) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="LogicCritterCountSensor"/>
    /// </summary>
    public ComponentResolver<LogicCritterCountSensor> Target => target;

    /// <summary>
    /// Should count critters
    /// </summary>
    public bool CountCritters => countCritters;

    /// <summary>
    /// Should count eggs
    /// </summary>
    public bool CountEggs => countEggs;
}
