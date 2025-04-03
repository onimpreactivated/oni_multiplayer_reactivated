using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Setting new driver for chore
/// </summary>
[Serializable]
public class SetDriverChoreCommand : BaseCommandEvent
{
    /// <summary>
    /// Resolver for the <see cref="ChoreDriver"/>
    /// </summary>
    public ComponentResolver<ChoreDriver> DriverReference;

    /// <summary>
    /// Resolver for the <see cref="ChoreConsumer"/>
    /// </summary>
    public ComponentResolver<ChoreConsumer> ConsumerReference;

    /// <summary>
    /// Resolver for the <see cref="Chore"/>
    /// </summary>
    public ChoreResolver ChoreReference;

    /// <summary>
    /// Wrapped object that send into network
    /// </summary>
    public object Data;

    /// <summary>
    /// Setting new <paramref name="driver"/> for <paramref name="chore"/>
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="consumer"></param>
    /// <param name="chore"></param>
    /// <param name="data"></param>
    public SetDriverChoreCommand(ChoreDriver driver, ChoreConsumer consumer, Chore chore, object data)
    {
        DriverReference = driver.GetComponentResolver();
        ConsumerReference = consumer.GetComponentResolver();
        ChoreReference = chore?.GetResolver();
        Data = ArgumentUtils.WrapObject(data);
    }
}
