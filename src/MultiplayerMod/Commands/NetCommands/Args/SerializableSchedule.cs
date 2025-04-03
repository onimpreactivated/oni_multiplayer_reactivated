using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.NetCommands.Args;

/// <summary>
/// Serializable <see cref="Schedule"/>
/// </summary>
/// <remarks>
/// Creating <see cref="SerializableSchedule"/> from <paramref name="schedule"/>
/// </remarks>
/// <param name="schedule"></param>
[Serializable]
public class SerializableSchedule(global::Schedule schedule)
{
    /// <summary>
    /// Name of the <see cref="Schedule"/>
    /// </summary>
    public string Name { get; } = schedule.name;
    /// <summary>
    /// Is the alarm active for <see cref="Schedule"/>
    /// </summary>
    public bool AlarmActivated { get; } = schedule.alarmActivated;
    private readonly List<ComponentResolver<Schedulable>> assigned = schedule.assigned
            .Select(@ref => @ref.obj.gameObject.GetComponent<Schedulable>().GetComponentResolver())
            .ToList();
    private readonly List<string> blocks = schedule.blocks.Select(block => block.GroupId).ToList();

    private static readonly Dictionary<string, ScheduleGroup> groups =
        Db.Get().ScheduleGroups.allGroups.ToDictionary(
            a => a.Id,
            // It is a group for 1 hour, so it's important to change defaultSegments value to '1' from the default.
            a => new ScheduleGroup(
                a.Id,
                null,
                1,
                a.Name,
                a.description,
                a.uiColor,
                a.notificationTooltip,
                a.allowedTypes,
                a.alarm
            )
        );

    /// <summary>
    /// A <see cref="ScheduleGroup"/> that inside the <see cref="Schedule"/> 
    /// </summary>
    public List<ScheduleGroup> Groups => blocks.Select(block => groups[block]).ToList();

    /// <summary>
    /// Assigned <see cref="Schedule"/> referece list
    /// </summary>
    public List<Ref<Schedulable>> Assigned => assigned.Select(reference => new Ref<Schedulable>(reference.Resolve())).ToList();
}
