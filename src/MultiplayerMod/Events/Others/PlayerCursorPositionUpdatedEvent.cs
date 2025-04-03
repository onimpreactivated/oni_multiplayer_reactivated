using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.EventArgs;

namespace MultiplayerMod.Events.Others;

/// <summary>
/// An event that updates the player mouse position
/// </summary>
/// <param name="player"></param>
/// <param name="eventArgs"></param>
public class PlayerCursorPositionUpdatedEvent(CorePlayer player, MouseMovedEventArgs eventArgs) : BaseEvent
{
    /// <summary>
    /// Player who's mouse to update
    /// </summary>
    public CorePlayer Player => player;

    /// <summary>
    /// Mouse Moved Event
    /// </summary>
    public MouseMovedEventArgs EventArgs => eventArgs;
}
