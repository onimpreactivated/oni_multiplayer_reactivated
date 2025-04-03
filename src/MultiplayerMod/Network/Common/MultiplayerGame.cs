using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Network.Common;

/// <summary>
/// Game that handles Players and Objects
/// </summary>
public class MultiplayerGame
{
    /// <summary>
    /// Player Mode using <see cref="PlayerRole"/>
    /// </summary>
    public PlayerRole Mode { get; private set; }

    /// <summary>
    /// List of current <see cref="CorePlayer"/>
    /// </summary>
    public CorePlayers Players { get; private set; }

    /// <summary>
    /// List of current <see cref="MultiplayerObject"/>
    /// </summary>
    public MultiplayerObjects Objects { get; private set; }

    /// <summary>
    /// Creates a new Multiplayer Game
    /// </summary>
    /// <param name="multiplayerObjects"></param>
    public MultiplayerGame(MultiplayerObjects multiplayerObjects)
    {
        Objects = multiplayerObjects;
        Refresh(PlayerRole.Client);
    }

    /// <summary>
    /// Refresh players with <paramref name="mode"/>
    /// </summary>
    /// <param name="mode"></param>
    public void Refresh(PlayerRole mode)
    {
        Mode = mode;
        Players = [];
    }
}
