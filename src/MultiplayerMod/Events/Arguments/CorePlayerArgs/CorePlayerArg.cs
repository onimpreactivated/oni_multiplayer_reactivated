using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Events.Arguments.CorePlayerArgs;

public class CorePlayerArg(CorePlayer corePlayer) : EventArgs
{
    public CorePlayer CorePlayer { get; } = corePlayer;
}
