using System.Reflection;

namespace MultiplayerMod.Events.Arguments;

public class StateMachineEventsArgs(StateMachine.Instance stateMachineInstance, MethodBase method, object[] args)
{
    /// <summary>
    /// Instance of the State Machine
    /// </summary>
    public StateMachine.Instance StateMachineInstance => stateMachineInstance;

    /// <summary>
    /// The original method.
    /// </summary>
    public MethodBase Method => method;

    /// <summary>
    /// Argument the event called
    /// </summary>
    public object[] Args => args;
}
