using MultiplayerMod.Commands;
using MultiplayerMod.Events.Arguments.CorePlayerArgs;
using MultiplayerMod.Events.Handlers;
using UnityEngine;

namespace MultiplayerMod.Core.Behaviour;

/// <summary>
/// Notifying players with specified messages
/// </summary>
public class MulitplayerNotifier : MonoBehaviour
{
    private const float notificationTimeout = 10f;
    private readonly LinkedList<Notification> notifications = new();
    private bool removalPending;

    internal void Awake()
    {
        PlayerEvents.PlayerLeft += OnPlayerLeft;
        NotificationManager.Instance.notificationRemoved += OnNotificationRemoved;
    }

    internal void OnDestroy()
    {
        if (NotificationManager.Instance != null)
            NotificationManager.Instance.notificationRemoved -= OnNotificationRemoved;
        PlayerEvents.PlayerLeft -= OnPlayerLeft;
    }

    private void OnPlayerLeft(CorePlayerLeftArg @event)
    {
        var playerName = @event.CorePlayer.Profile.PlayerName;
        var message = $"{playerName} left";
        var description = $"{playerName} {(@event.IsForced ? "left" : "disconnected")}";
        AddNotification(message, description, NotificationType.BadMinor);
    }

    /// <summary>
    /// Add notification to screen
    /// </summary>
    /// <param name="message"></param>
    /// <param name="tooltip"></param>
    /// <param name="type"></param>
    public void AddNotification(string message, string tooltip, NotificationType type)
    {
        var notification = new Notification(
            message,
            type,
            tooltip: (_, _) => tooltip,
            expires: false,
            clear_on_click: true,
            show_dismiss_button: true
        )
        {
            GameTime = Time.unscaledTime,
            Time = KTime.Instance.UnscaledGameTime,
            Delay = -Time.unscaledTime
        };
        NotificationManager.Instance.AddNotification(notification);
        notifications.AddLast(notification);
    }

    private void OnNotificationRemoved(Notification notification)
    {
        if (!removalPending)
            notifications.Remove(notification);
    }

    internal void Update()
    {
        List<Notification> toRemove = [];
        for (int i = 0; i < notifications.Count; i++)
        {
            var notification = notifications.ElementAt(i);
            if (Time.unscaledTime - notification.GameTime < notificationTimeout)
                return;

            removalPending = true;
            NotificationManager.Instance.RemoveNotification(notification);
            toRemove.Add(notification);
            removalPending = false;
        }
        toRemove.ForEach(x => notifications.Remove(x));
    }

}
