namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Permit the <paramref name="permittedList"/> to not be consume by default
/// </summary>
/// <param name="permittedList"></param>
[Serializable]
public class PermitConsumableByDefaultCommand(List<Tag> permittedList) : BaseCommandEvent
{
    /// <summary>
    /// The list that must not to consume
    /// </summary>
    public List<Tag> PermittedList => permittedList;
}
