using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Permit the <paramref name="consumableId"/> to <paramref name="instance"/>
/// </summary>
/// <param name="instance"></param>
/// <param name="consumableId"></param>
/// <param name="isAllowed"></param>
[Serializable]
public class PermitConsumableToMinionCommand(GameObjectResolver instance, string consumableId, bool isAllowed) : BaseCommandEvent
{
    /// <summary>
    /// The <see cref="ConsumableConsumer"/> instance
    /// </summary>
    public GameObjectResolver Instance => instance;

    /// <summary>
    /// Id of the consumable
    /// </summary>
    public string ConsumableId => consumableId;

    /// <summary>
    /// Can consume this <see cref="ConsumableId"/>
    /// </summary>
    public bool IsAllowed => isAllowed;
}
