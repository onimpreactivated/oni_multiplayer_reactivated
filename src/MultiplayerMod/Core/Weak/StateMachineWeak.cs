using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Core.Exceptions;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Weak;

/// <summary>
/// Weak table for <see cref="StateMachine.Instance"/>
/// </summary>
public class StateMachineWeak
{
    private static readonly ConditionalWeakTable<StateMachine.Instance, StateMachineWeak> cache = new();

    private readonly StateMachine.Instance instance;

    private StateMachineWeak(StateMachine.Instance instance)
    {
        this.instance = instance;
    }

    /// <summary>
    /// Find <see cref="Parameter{T}"/> with the providen <paramref name="parameterInfo"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameterInfo"></param>
    /// <returns></returns>
    public Parameter<T> FindParameter<T>(ParameterInfo<T> parameterInfo)
    {
        var parameters = instance.stateMachine.parameters;

        // There are not many parameters, caching is probably unjustified
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].name == parameterInfo.Name)
                return new Parameter<T>(instance, parameters[i]);
        }
        return null;
    }

    /// <summary>
    /// Make <see cref="instance"/> go to desired state <paramref name="name"/>
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="StateMachineStateNotFoundException"></exception>
    public void GoToState(string name)
    {
        if (name != null)
        {
            var state = instance.stateMachine.GetState(name) ?? throw new StateMachineStateNotFoundException(instance.stateMachine, name);
            instance.GoTo(state);
        }
        else
            instance.GoTo((StateMachine.BaseState)null);
    }

    /// <summary>
    /// Get the <see cref="StateMachineController"/> from <see cref="instance"/>
    /// </summary>
    /// <returns></returns>
    public StateMachineController GetController() => (StateMachineController) instance.GetFieldValue(nameof(StateMachineMemberReference.Instance.controller));

    /// <summary>
    /// Get <see cref="StateMachineWeak"/> from <paramref name="instance"/>
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static StateMachineWeak Get(StateMachine.Instance instance)
    {
        if (cache.TryGetValue(instance, out var tools))
            return tools;
        tools = new StateMachineWeak(instance);
        cache.Add(instance, tools);
        return tools;
    }
}
