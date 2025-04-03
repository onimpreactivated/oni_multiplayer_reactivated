namespace MultiplayerMod.Core.Execution;

/// <summary>
/// 
/// </summary>
public enum ExecutionLevel
{

    /// <summary>
    /// This level is always active. <br/>
    /// What should be used at that level: mod initialization components that don't depend on a game mode (single or multiplayer).
    /// </summary>
    System,

    /// <summary>
    /// This level is active when the game mode is multiplayer. <br/>
    /// What should be used at that level: components that only active in the multiplayer game.
    /// </summary>
    Multiplayer,

    /// <summary>
    /// This level is active when a game component is being initialized. <br/>
    /// It's used as a suppression mechanism of command production.
    /// </summary>
    Component,

    /// <summary>
    /// This level is active during command execution.
    /// </summary>
    Command,

    /// <summary>
    /// This level is active by default when a multiplayer game is created and running. <br/>
    /// What should be used at that level: all game events that produce commands.
    /// </summary>
    Game

}
