namespace MultiplayerMod.Core.Player;

/// <summary>
/// Basic profile that represent a Player
/// </summary>
/// <param name="playerName"></param>
[Serializable]
public class PlayerProfile(string playerName)
{
    /// <summary>
    /// The name of the player
    /// </summary>
    public string PlayerName => playerName;
}
