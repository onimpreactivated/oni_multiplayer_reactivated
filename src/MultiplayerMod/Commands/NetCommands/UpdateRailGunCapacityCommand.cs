using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Event that lets update <see cref="RailGun"/>'s capacity
/// </summary>
[Serializable]
public class UpdateRailGunCapacityCommand(ComponentResolver<RailGun> target, float launchMass) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="RailGun"/>
    /// </summary>
    public ComponentResolver<RailGun> Target => target;

    /// <summary>
    /// New launch mass to set
    /// </summary>
    public float LaunchMass = launchMass;
}
