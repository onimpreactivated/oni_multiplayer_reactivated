namespace MultiplayerMod.Network.Common;

/// <summary>
/// Networking state of the Server
/// </summary>
public enum NetStateServer
{
    /// <summary>
    /// Server received an Error during operation
    /// </summary>
    Error = -1,
    /// <summary>
    /// Server stopped hosting the game
    /// </summary>
    Stopped,
    /// <summary>
    /// Server preparing to start the game
    /// </summary>
    Preparing,
    /// <summary>
    /// Server is starting the game
    /// </summary>
    Starting,
    /// <summary>
    /// Server is started the game
    /// </summary>
    Started
}
