using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;


[Serializable]
public class ChangeSchedulesListCommand : BaseCommandEvent
{
    /// <summary>
    /// <see cref="SerializableSchedule"/> to send over network. And also set to clients.
    /// </summary>
    public List<SerializableSchedule> SerializableSchedules { get; }

    public ChangeSchedulesListCommand(List<global::Schedule> schedules)
    {
        SerializableSchedules = schedules.Select(schedule => new SerializableSchedule(schedule)).ToList();
    }
}
