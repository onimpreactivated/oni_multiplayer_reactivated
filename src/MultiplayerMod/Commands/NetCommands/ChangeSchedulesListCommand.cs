using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Command that changing to <paramref name="schedules"/>
/// </summary>
/// <param name="schedules"></param>
[Serializable]
public class ChangeSchedulesListCommand(List<global::Schedule> schedules) : BaseCommandEvent
{
    /// <summary>
    /// <see cref="SerializableSchedule"/> to send over network. And also set to clients.
    /// </summary>
    public List<SerializableSchedule> SerializableSchedules => schedules.Select(schedule => new SerializableSchedule(schedule)).ToList();
}
