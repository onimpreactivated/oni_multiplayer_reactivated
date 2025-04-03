namespace MultiplayerMod.Events.Common;

/// <summary>
/// Event that tells the server/client that the world currently loading
/// </summary>
public class WorldLoadingEvent : BaseEvent;

/// <summary>
/// Event that tells the server/client that the world currently syncing
/// </summary>
public class WorldSyncEvent : BaseEvent;

/// <summary>
/// Request the server to do World Syncing
/// </summary>
public class WorldSyncRequestedEvent : BaseEvent;

/// <summary>
/// Event that tells clients the world saved.
/// </summary>
public class WorldSavedEvent : BaseEvent;

/// <summary>
/// Player guitting the game
/// </summary>
public class GameQuitEvent : BaseEvent;

/// <summary>
/// Game is in playable state
/// </summary>
public class GameReadyEvent : BaseEvent;

/// <summary>
/// Game is started
/// </summary>
public class GameStartedNoArgsEvent : BaseEvent;

/// <summary>
/// Choosen single player mode to play
/// </summary>
public class SinglePlayerModeSelectedEvent : BaseEvent;

/// <summary>
/// Main Menu has been Initialized
/// </summary>
public class MainMenuInitialized : BaseEvent;

/// <summary>
/// Connection has been lost to a peer
/// </summary>
public class ConnectionLostEvent : BaseEvent;

/// <summary>
/// World has started initializing.
/// </summary>
public class WorldStateInitializingEvent : BaseEvent;
