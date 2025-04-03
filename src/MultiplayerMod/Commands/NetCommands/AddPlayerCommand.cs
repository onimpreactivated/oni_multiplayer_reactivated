using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Add player into the network
/// </summary>
/// <param name="player"></param>
/// <param name="current"></param>
[Serializable]
public class AddPlayerCommand(CorePlayer player, bool current) : BaseCommandEvent
{
    /// <summary>
    /// The Player to add
    /// </summary>
    public CorePlayer Player => player;

    /// <summary>
    /// Is Current player.
    /// </summary>
    public bool Current => current;
}
