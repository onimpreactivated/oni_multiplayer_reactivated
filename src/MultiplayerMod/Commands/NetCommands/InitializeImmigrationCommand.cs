namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Initialize Deliverables to all clients.
/// </summary>
/// <param name="deliverables"></param>
[Serializable]
public class InitializeImmigrationCommand(List<ITelepadDeliverable> deliverables) : BaseCommandEvent
{
    /// <summary>
    /// The Deliverables want to sync.
    /// </summary>
    public List<ITelepadDeliverable> Deliverables => deliverables;
}
