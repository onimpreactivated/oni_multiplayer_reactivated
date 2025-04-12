using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class ReleaseChoreDriverCommand : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="ChoreDriver"/>
    /// </summary>
    public ComponentResolver<ChoreDriver> DriverReference { get; }

    /// <summary>
    /// Called when the driver no longer needed for the <see cref="Chore"/>
    /// </summary>
    /// <param name="driver"></param>
    public ReleaseChoreDriverCommand(ChoreDriver driver)
    {
        DriverReference = driver.GetComponentResolver();
    }
}
