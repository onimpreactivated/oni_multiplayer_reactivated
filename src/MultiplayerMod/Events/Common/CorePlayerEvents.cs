using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Events.Common;

/// <summary>
/// Initialize current player
/// </summary>
/// <param name="player"></param>
public class CurrentPlayerInitializedEvent(CorePlayer player) : BaseEvent
{
    /// <summary>
    /// The Player
    /// </summary>
    public CorePlayer Player => player;
}

/// <summary>
/// Joining new player Event
/// </summary>
/// <param name="player"></param>
public class PlayerJoinedEvent(CorePlayer player) : BaseEvent
{
    /// <summary>
    /// The Player
    /// </summary>
    public CorePlayer Player => player;
}

/// <summary>
/// Leaving player Event
/// </summary>
/// <param name="player"></param>
/// <param name="isForced"></param>
public class PlayerLeftEvent(CorePlayer player, bool isForced) : BaseEvent
{
    /// <summary>
    /// The Player
    /// </summary>
    public CorePlayer Player => player;

    /// <summary>
    /// Is forced to disconnect
    /// </summary>
    public bool IsForced => isForced;
}

/// <summary>
/// Player changing new state Event
/// </summary>
/// <param name="player"></param>
/// <param name="state"></param>
public class PlayerStateChangedEvent(CorePlayer player, PlayerState state) : BaseEvent
{
    /// <summary>
    /// The Player
    /// </summary>
    public CorePlayer Player => player;

    /// <summary>
    /// New State
    /// </summary>
    public PlayerState State => state;
}

/// <summary>
/// Player list update Event
/// </summary>
/// <param name="players"></param>
public class PlayersUpdatedEvent(CorePlayers players) : BaseEvent
{
    /// <summary>
    /// List of Players
    /// </summary>
    public CorePlayers Players => players;
}
