using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands.Args;

/// <summary>
/// Argument collection for <see cref="UpdateAlarmCommand"/>
/// </summary>
/// <param name="target"></param>
/// <param name="notificationName"></param>
/// <param name="notificationTooltip"></param>
/// <param name="pauseOnNotify"></param>
/// <param name="zoomOnNotify"></param>
/// <param name="notificationType"></param>
[Serializable]
public class AlarmSideScreenEventArgs(
        ComponentResolver<LogicAlarm> target,
        string notificationName,
        string notificationTooltip,
        bool pauseOnNotify,
        bool zoomOnNotify,
        NotificationType notificationType)
{
    /// <summary>
    /// <see cref="LogicAlarm"/> Resolver
    /// </summary>
    public ComponentResolver<LogicAlarm> Target => target;

    /// <summary>
    /// The name of Notification
    /// </summary>
    public string NotificationName => notificationName;

    /// <summary>
    /// The toooltip for the Notification
    /// </summary>
    public string NotificationTooltip => notificationTooltip;

    /// <summary>
    /// Should the game pause when notify received
    /// </summary>
    public bool PauseOnNotify => pauseOnNotify;

    /// <summary>
    /// Zoom to place where notify happened
    /// </summary>
    public bool ZoomOnNotify => zoomOnNotify;

    /// <summary>
    /// The <see cref="NotificationType"/> for this Notification
    /// </summary>
    public NotificationType NotificationType => notificationType;
}
