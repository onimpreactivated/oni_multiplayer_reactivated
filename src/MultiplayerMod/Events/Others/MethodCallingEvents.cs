using MultiplayerMod.Events.EventArgs;

namespace MultiplayerMod.Events.Others;

/// <summary>
/// Event that called when a component calling a patched method
/// </summary>
/// <param name="args"></param>
public class ComponentMethodCalled(ComponentEventsArgs args) : BaseEvent
{
    /// <summary>
    /// The argument that the method called
    /// </summary>
    public ComponentEventsArgs Args => args;
}

/// <summary>
/// Event that called when a state machine calling a patched method
/// </summary>
/// <param name="args"></param>
public class StateMachineMethodCalled(StateMachineEventsArgs args) : BaseEvent
{
    /// <summary>
    /// The argument that the method called
    /// </summary>
    public StateMachineEventsArgs Args => args;
}
