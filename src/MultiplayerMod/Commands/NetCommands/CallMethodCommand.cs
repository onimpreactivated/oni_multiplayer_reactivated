using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Events.EventArgs;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Calling a specific method over command
/// </summary>
[Serializable]
public class CallMethodCommand : BaseCommandEvent
{
    /// <summary>
    /// The resolved target
    /// </summary>
    public readonly IResolver target;
    /// <summary>
    /// Method declared type
    /// </summary>
    public readonly Type declaringType;
    /// <summary>
    /// The Method name to call
    /// </summary>
    public readonly string methodName;
    /// <summary>
    /// Arguments to call.
    /// </summary>
    public readonly object[] args;

    /// <summary>
    /// Create a command caller with <see cref="ComponentEventsArgs"/>
    /// </summary>
    /// <param name="eventArgs"></param>
    public CallMethodCommand(ComponentEventsArgs eventArgs)
    {
        target = eventArgs.Component.GetComponentResolver();
        declaringType = eventArgs.Method.DeclaringType!;
        methodName = eventArgs.Method.Name;
        args = ArgumentUtils.WrapObjects(eventArgs.Args);
    }

    /// <summary>
    /// Create a command caller with <see cref="StateMachineEventsArgs"/>
    /// </summary>
    /// <param name="eventArgs"></param>
    public CallMethodCommand(StateMachineEventsArgs eventArgs)
    {
        target = eventArgs.StateMachineInstance.GetSMResolver();
        declaringType = eventArgs.Method.DeclaringType!;
        methodName = eventArgs.Method.Name;
        args = ArgumentUtils.WrapObjects(eventArgs.Args);
    }

    /// <inheritdoc/>
    public override string ToString() => $"{base.ToString()} (Type = {declaringType.FullName}, Method = {methodName})";
}
