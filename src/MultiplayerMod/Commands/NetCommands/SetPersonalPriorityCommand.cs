using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Set the priorty <paramref name="value"/> to the <paramref name="id"/> that has the ChoreConsumer as <paramref name="resolver"/>
/// </summary>
/// <param name="resolver"></param>
/// <param name="id"></param>
/// <param name="value"></param>
[Serializable]
public class SetPersonalPriorityCommand(GameObjectResolver resolver, string id, int value) : BaseCommandEvent
{
    /// <summary>
    /// Reference to <see cref="ChoreConsumer"/>
    /// </summary>
    public GameObjectResolver ChoreConsumerReference => resolver;

    /// <summary>
    /// The Chores Group Id
    /// </summary>
    public string ChoreGroupId => id;

    /// <summary>
    /// The Priority
    /// </summary>
    public int Value => value;

    /// <summary>
    /// Getting the ChoreGroup from <see cref="ChoreGroupId"/>
    /// </summary>
    public ChoreGroup ChoreGroup => Db.Get().ChoreGroups.resources.FirstOrDefault(resource => resource.Id == ChoreGroupId);
}
