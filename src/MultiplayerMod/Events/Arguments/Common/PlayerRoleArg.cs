using MultiplayerMod.Core.Player;

namespace MultiplayerMod.Events.Arguments.Common;

public class PlayerRoleArg(PlayerRole value) : EventArgs
{
    public PlayerRole Value { get; } = value;
}
