using System.Reflection;

namespace MultiplayerMod.Events.Arguments;

/// <summary>
/// Arguments supplied for <see cref="Others.StateMachineMethodCalled"/>
/// </summary>
/// <param name="stateMachineInstance"></param>
/// <param name="method"></param>
/// <param name="args"></param>
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
