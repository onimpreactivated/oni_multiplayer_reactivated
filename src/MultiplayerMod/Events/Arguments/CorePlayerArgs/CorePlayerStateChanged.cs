using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Events.Arguments.CorePlayerArgs;

public class CorePlayerStateChanged(CorePlayer player, PlayerState state) : CorePlayerArg(player)
{
    public PlayerState State { get; } = state;
}
