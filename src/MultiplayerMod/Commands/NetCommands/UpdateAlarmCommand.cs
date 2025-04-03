using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Update <see cref="LogicAlarm"/>
/// </summary>
/// <param name="args"></param>
[Serializable]
public class UpdateAlarmCommand(AlarmSideScreenEventArgs args) : BaseCommandEvent
{
    /// <summary>
    /// The <see cref="AlarmSideScreenEventArgs"/> for update
    /// </summary>
    public AlarmSideScreenEventArgs Args => args;
}
