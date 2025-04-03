using MultiplayerMod.Events.EventArgs;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Event that updates <paramref name="playerId"/> Mouse position
/// </summary>
/// <param name="playerId"></param>
/// <param name="eventArgs"></param>
[Serializable]
public class UpdatePlayerCursorPositionCommand(Guid playerId, MouseMovedEventArgs eventArgs) : BaseCommandEvent
{
    /// <summary>
    /// Who send the update
    /// </summary>
    public Guid PlayerId => playerId;

    /// <summary>
    /// Mouse Moved Event
    /// </summary>
    public MouseMovedEventArgs EventArgs => eventArgs;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{base.ToString()} | Id: {PlayerId}, Args: {EventArgs}";
    }
}
