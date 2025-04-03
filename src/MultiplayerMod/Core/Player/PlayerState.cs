namespace MultiplayerMod.Core.Player;

/// <summary>
/// Representing a State of a Player
/// </summary>
public enum PlayerState : byte
{
    /// <summary>
    /// Player is initialzing the game, classes
    /// </summary>
    Initializing,
    /// <summary>
    /// Player is loading into the game
    /// </summary>
    Loading,
    /// <summary>
    /// Player is ready
    /// </summary>
    Ready,
    /// <summary>
    /// Player is leaving
    /// </summary>
    Leaving
}
