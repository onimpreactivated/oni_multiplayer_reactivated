using MultiplayerMod.Core.Wrappers;
using System.Reflection;

namespace MultiplayerMod.ChoreSync.StateMachines;

/// <summary>
/// <paramref name="parameter"/> setter for <paramref name="instance"/>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="instance"></param>
/// <param name="parameter"></param>
public class Parameter<T>(StateMachine.Instance instance, StateMachine.Parameter parameter)
{
    private readonly MethodBase setter = parameter.GetType().GetMethod(nameof(StateMachineMemberReference.Parameter.Set))!;

    /// <summary>
    /// Set the <paramref name="value"/> to the Parameter
    /// </summary>
    /// <param name="value"></param>
    /// <param name="silenceEvents"></param>
    public void Set(T value, bool silenceEvents = false) => setter.Invoke(parameter, [value, instance, silenceEvents]);
}
