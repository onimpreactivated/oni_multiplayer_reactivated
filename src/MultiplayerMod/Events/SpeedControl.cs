using MultiplayerMod.Events.Arguments.Common;

namespace MultiplayerMod.Events;

public static class SpeedControl
{
    public static event OniEventHandlerTEventArgs<IntArg> SpeedControlSetSpeed;
    public static event OniEventHandler SpeedControlPause;
    public static event OniEventHandler SpeedControlResume;



    public static void OnSpeedControlSetSpeed(int speed)
    {
        SpeedControlSetSpeed?.Invoke(new(speed));
    }

    public static void OnSpeedControlPause()
    {
        SpeedControlPause?.Invoke();
    }

    public static void OnSpeedControlResume()
    {
        SpeedControlResume?.Invoke();
    }
}
