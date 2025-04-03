using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Events.MainMenu;

/// <summary>
/// Creating a new multiplayer with default as <paramref name="role"/>
/// </summary>
/// <param name="role"></param>
public class MultiplayerModeSelectedEvent(PlayerRole role) : BaseEvent
{
    /// <summary>
    /// Default role for the Player
    /// </summary>
    public PlayerRole Role => role;
}
