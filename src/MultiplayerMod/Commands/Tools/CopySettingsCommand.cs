using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// Command for <see cref="CopySettingsTool"/>
/// </summary>
/// <param name="args"></param>
[Serializable]
public class CopySettingsCommand(CopySettingsEventArgs args) : BaseCommandEvent
{
    /// <summary>
    /// Argument for <see cref="CopySettingsTool"/>
    /// </summary>
    public CopySettingsEventArgs Args => args;
}
