namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Remove player from the network
/// </summary>
/// <param name="playerId"></param>
/// <param name="reason"></param>
[Serializable]
public class RemovePlayerCommand(Guid playerId, string reason) : BaseCommandEvent
{
    /// <summary>
    /// To be remove player Id
    /// </summary>
    public Guid PlayerId => playerId;

    /// <summary>
    /// Reason player has been removed
    /// </summary>
    public string ReasonToRemove => reason;
}
