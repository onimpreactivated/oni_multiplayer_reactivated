namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Setting advanced Priorites to the game
/// </summary>
/// <param name="isAdvanced"></param>
[Serializable]
public class SetPersonalPrioritiesAdvancedCommand(bool isAdvanced) : BaseCommandEvent
{
    /// <summary>
    /// Should be Advenced
    /// </summary>
    public bool IsAdvanced => isAdvanced;
}
