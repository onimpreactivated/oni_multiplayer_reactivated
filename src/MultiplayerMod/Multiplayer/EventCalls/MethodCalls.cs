using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Others;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class MethodCalls
{
    internal static void ComponentMethodCalled_Event(ComponentMethodCalled called)
    {
        if (MultiplayerManager.Instance.NetClient == null)
            return;
        MultiplayerManager.Instance.NetClient.Send(new CallMethodCommand(called.Args));
    }

    internal static void ComponentMethodCalled_Event(StateMachineMethodCalled called)
    {
        if (MultiplayerManager.Instance.NetClient == null)
            return;
        MultiplayerManager.Instance.NetClient.Send(new CallMethodCommand(called.Args));
    }
}
