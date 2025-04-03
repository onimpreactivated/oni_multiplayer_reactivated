using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Command that set the <paramref name="skillId"/> to a <paramref name="minionIdentityReference"/>
/// </summary>
/// <param name="minionIdentityReference"></param>
/// <param name="skillId"></param>
[Serializable]
public class MasterSkillCommand(GameObjectResolver minionIdentityReference, string skillId) : BaseCommandEvent
{
    /// <summary>
    /// <see cref="GameObjectResolver"/> for <see cref="MinionIdentity"/>
    /// </summary>
    public GameObjectResolver MinionIdentityReference => minionIdentityReference;

    /// <summary>
    /// 
    /// </summary>
    public string SkillId => skillId;
}
