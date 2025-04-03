namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Set the default priorty <paramref name="value"/> to the <paramref name="id"/>
/// </summary>
/// <param name="id"></param>
/// <param name="value"></param>
[Serializable]
public class SetDefaultPriorityCommand(string id, int value) : BaseCommandEvent
{
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
