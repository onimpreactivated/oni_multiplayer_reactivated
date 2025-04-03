using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Others;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class SpeedCalls
{
    internal static void SpeedControlSetSpeed_Event(SpeedControlSetSpeed speed)
    {
        MultiplayerManager.Instance.NetClient.Send(new ChangeGameSpeed(speed.Speed));
    }

    internal static void SpeedControlPause_Event(SpeedControlPause _)
    {
        MultiplayerManager.Instance.NetClient.Send(new PauseGameCommand());
    }

    internal static void SpeedControlResume_Event(SpeedControlResume _)
    {
        MultiplayerManager.Instance.NetClient.Send(new ResumeGameCommand());
    }
}
