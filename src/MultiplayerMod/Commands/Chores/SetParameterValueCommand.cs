using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Core.Weak;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Called for setting a parameter for all client.
/// </summary>
[Serializable]
public class SetParameterValueCommand : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="StateMachineController"/>
    /// </summary>
    public ComponentResolver<StateMachineController> Controller { get; }

    /// <summary>
    /// Type for the StateMachine
    /// </summary>
    public Type StateMachineInstanceType { get; }

    /// <summary>
    /// Parameter index
    /// </summary>
    public int ParameterIndex { get; }

    /// <summary>
    /// Parameter Value
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Called for setting a parameter for all client.
    /// </summary>
    /// <param name="smi"></param>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    public SetParameterValueCommand(StateMachine.Instance smi, StateMachine.Parameter parameter, object value)
    {
        var runtime = StateMachineWeak.Get(smi);
        Controller = runtime.GetController().GetComponentResolver();
        StateMachineInstanceType = smi.GetType();
        ParameterIndex = parameter.idx;
        this.Value = ArgumentUtils.WrapObject(value);
    }
}
