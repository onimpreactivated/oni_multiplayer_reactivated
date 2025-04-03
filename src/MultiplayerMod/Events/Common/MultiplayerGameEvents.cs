namespace MultiplayerMod.Events.Common;

/// <summary>
/// Multiplayer stated event
/// </summary>
public class GameStartedEvent : BaseEvent;

/// <summary>
/// Stop multiplayer session.
/// </summary>
public class StopMultiplayerEvent : BaseEvent;

/// <summary>
/// Called when all players are ready
/// </summary>
public class PlayersReadyEvent : BaseEvent;
