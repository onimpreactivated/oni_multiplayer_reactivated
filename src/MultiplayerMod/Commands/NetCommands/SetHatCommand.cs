using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Command that set the <paramref name="minionIdentityReference"/> to a hat is <paramref name="targetHat"/>
/// </summary>
/// <param name="minionIdentityReference"></param>
/// <param name="targetHat"></param>
[Serializable]
public class SetHatCommand(GameObjectResolver minionIdentityReference, string targetHat) : BaseCommandEvent
{
    /// <summary>
    /// <see cref="GameObjectResolver"/> for <see cref="MinionIdentity"/>
    /// </summary>
    public GameObjectResolver MinionIdentityReference => minionIdentityReference;

    /// <summary>
    /// Hat to set it, (can be null!)
    /// </summary>
    public string TargetHat => targetHat;
}
