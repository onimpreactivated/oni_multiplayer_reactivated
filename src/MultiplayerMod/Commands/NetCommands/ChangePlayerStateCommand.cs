using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Changing player state.
/// </summary>
/// <param name="playerId"></param>
/// <param name="state"></param>
[Serializable]
public class ChangePlayerStateCommand(Guid playerId, PlayerState state) : BaseCommandEvent
{
    /// <summary>
    /// Player Id
    /// </summary>
    public Guid PlayerId => playerId;

    /// <summary>
    /// The new state
    /// </summary>
    public PlayerState State => state;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{base.ToString()} | PlayerId: {PlayerId} State: {State}";
    }
}
