using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Called when the driver no longer needed for the <see cref="Chore"/>
/// </summary>
/// <param name="driver"></param>
[Serializable]
public class ReleaseChoreDriverCommand(ChoreDriver driver) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="ChoreDriver"/>
    /// </summary>
    public ComponentResolver<ChoreDriver> DriverReference => driver.GetComponentResolver();
}
