namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Enable <see cref="RedAlertMonitor"/> to show on screen
/// </summary>
/// <param name="isEnabled"></param>
[Serializable]
public class ChangeRedAlertStateCommand(bool isEnabled) : BaseCommandEvent
{
    /// <summary>
    /// Should the <see cref="RedAlertMonitor"/> be enabled.
    /// </summary>
    public bool IsEnabled => isEnabled;
}
