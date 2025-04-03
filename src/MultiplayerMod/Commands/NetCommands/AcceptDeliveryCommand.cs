using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Command that accepts a new Delivery. (Duplicant or Item)
/// </summary>
[Serializable]
public class AcceptDeliveryCommand(ComponentResolver<Telepad> target, ITelepadDeliverable deliverable, MultiplayerId gameObjectId, MultiplayerId proxyId) : BaseCommandEvent
{
    /// <summary>
    /// The Telepad component
    /// </summary>
    public ComponentResolver<Telepad> Target => target;

    /// <summary>
    /// What should be delivered
    /// </summary>
    public ITelepadDeliverable Deliverable => deliverable;
    /// <summary>
    /// The game objects <see cref="MultiplayerId"/>
    /// </summary>
    public MultiplayerId GameObjectId => gameObjectId;

    /// <summary>
    /// The <see cref="MultiplayerId"/> of <see cref="MinionIdentity"/> 
    /// </summary>
    public MultiplayerId ProxyId => proxyId;
}
