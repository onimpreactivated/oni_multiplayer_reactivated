namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Command to user change to specific <paramref name="speed"/>
/// </summary>
/// <param name="speed"></param>
[Serializable]
public class ChangeGameSpeed(int speed) : BaseCommandEvent
{
    /// <summary>
    /// Speed to change to.
    /// </summary>
    public int Speed => speed;
}
