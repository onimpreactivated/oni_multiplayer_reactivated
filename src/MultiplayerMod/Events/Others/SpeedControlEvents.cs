namespace MultiplayerMod.Events.Others;

/// <summary>
/// Event that called when user sets a new speed.
/// </summary>
/// <param name="speed"></param>
public class SpeedControlSetSpeed(int speed) : BaseEvent
{
    /// <summary>
    /// Speed the user set it to.
    /// </summary>
    public int Speed => speed;
}

/// <summary>
/// Event called when user pause tha game
/// </summary>
public class SpeedControlPause : BaseEvent;

/// <summary>
/// Event called when user resume tha game
/// </summary>
public class SpeedControlResume : BaseEvent;
