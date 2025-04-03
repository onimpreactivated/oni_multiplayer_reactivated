namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Notify all clients the world is preparing
/// </summary>
[Serializable]
public class NotifyWorldSavePreparingCommand : BaseCommandEvent;

/// <summary>
/// Requesting world sync from host
/// </summary>
[Serializable]
public class RequestWorldSyncCommand : BaseCommandEvent;

/// <summary>
/// Command that makes the game Pause
/// </summary>
[Serializable]
public class PauseGameCommand : BaseCommandEvent;

/// <summary>
/// Command that makes the game Resume
/// </summary>
[Serializable]
public class ResumeGameCommand : BaseCommandEvent;

/// <summary>
/// Command that is calling FrameStep
/// </summary>
[Serializable]
public class DebugGameFrameStepCommand : BaseCommandEvent;

/// <summary>
/// Command that is calling SimulationStep
/// </summary>
[Serializable]
public class DebugSimulationStepCommand : BaseCommandEvent;
