namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Set the disinfected settings.
/// </summary>
/// <param name="minGerm"></param>
/// <param name="enableAutoDisinfect"></param>
[Serializable]
public class SetDisinfectSettingsCommand(int minGerm, bool enableAutoDisinfect) : BaseCommandEvent
{
    /// <summary>
    /// Minimum germ to disinfect
    /// </summary>
    public int MinGerm => minGerm;

    /// <summary>
    /// Enable auto disinfect for this save.
    /// </summary>
    public bool EnableAutoDisinfect => enableAutoDisinfect;
}
