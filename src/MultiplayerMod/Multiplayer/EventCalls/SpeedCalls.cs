using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Arguments.Common;
using MultiplayerMod.Events.Handlers;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class SpeedCalls : BaseEventCall
{
    public override void Init()
    {
        SpeedControlEvents.SpeedControlResume += SpeedControlResume_Event;
        SpeedControlEvents.SpeedControlPause += SpeedControlPause_Event;
        SpeedControlEvents.SpeedControlSetSpeed += SpeedControlSetSpeed_Event;
    }

    internal static void SpeedControlSetSpeed_Event(IntArg speed)
    {
        MultiplayerManager.Instance.NetClient.Send(new ChangeGameSpeed(speed.Value));
    }

    internal static void SpeedControlPause_Event()
    {
        MultiplayerManager.Instance.NetClient.Send(new PauseGameCommand());
    }

    internal static void SpeedControlResume_Event()
    {
        MultiplayerManager.Instance.NetClient.Send(new ResumeGameCommand());
    }
}
