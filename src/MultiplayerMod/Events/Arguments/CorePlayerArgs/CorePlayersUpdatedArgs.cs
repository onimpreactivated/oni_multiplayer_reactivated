using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Events.Arguments.CorePlayerArgs;

public class CorePlayersUpdatedArgs(CorePlayers players) : EventArgs
{
    public CorePlayers Players { get; } = players;
}
