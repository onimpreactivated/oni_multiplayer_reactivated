using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Events.Arguments.CorePlayerArgs;

public class CorePlayerLeftArg(CorePlayer player, bool isForced) : CorePlayerArg(player)
{
    public bool IsForced { get; } = isForced;
}
