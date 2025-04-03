namespace MultiplayerMod.Core.Player;

/// <summary>
/// Server/Client representation
/// </summary>
public enum PlayerRole : byte
{
    /// <summary>
    /// Player is Hosting the gane
    /// </summary>
    Server,
    /// <summary>
    /// Player is Connected to a host
    /// </summary>
    Client
}
