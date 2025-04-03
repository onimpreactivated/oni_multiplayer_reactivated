using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Command that rejects a new Delivery. (Duplicant or Item) 
/// </summary>
/// <param name="target"></param>
[Serializable]
public class RejectDeliveryCommand(ComponentResolver<Telepad> target) : BaseCommandEvent
{
    /// <summary>
    /// The Telepad target use to reject it.
    /// </summary>
    public ComponentResolver<Telepad> Target => target;
}
