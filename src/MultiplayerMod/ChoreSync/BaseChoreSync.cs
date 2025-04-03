using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.ChoreSync;

internal abstract class BaseChoreSync<ChoreStateMachine> : IChoreSync
    where ChoreStateMachine : StateMachine
{
    public Type StateMachineType => typeof(ChoreStateMachine);
    public abstract Type SyncType { get; }
    public ChoreStateMachine SM { get; internal set; }
    public abstract void Client(StateMachine instance);
    public abstract void Server(StateMachine instance);

    public void Setup(StateMachine __instance)
    {
        SM = __instance as ChoreStateMachine;
    }

    public Param AddMultiplayerParameter<T, Param>(ParameterInfo<T> parameterInfo) where Param : StateMachine.Parameter, new()
    {
        if (parameterInfo.Shared)
        {
            var existingParameter = SM.parameters.FirstOrDefault(it => it.name == parameterInfo.Name);
            if (existingParameter != null)
                return (Param) existingParameter;
        }
        var parameter = new Param
        {
            name = parameterInfo.Name,
            idx = SM.parameters.Length
        };
        SM.parameters = SM.parameters.Append(parameter);
        return parameter;
    }

    public Returner AddState<Parent, Returner>(Parent parent, StateInfo stateInfo, Action<Parent, Returner, string> callback)
        where Parent : StateMachine.BaseState, new()
        where Returner : StateMachine.BaseState, new()
    {
        return AddState(parent, stateInfo.Name, callback);
    }

    public Returner AddState<Parent, Returner>(Parent parent, string name, Action<Parent, Returner, string> callback)
        where Parent : StateMachine.BaseState, new()
        where Returner : StateMachine.BaseState, new()
    {
        if (parent.GetFieldValue("sm") != SM)
            throw new Exception($"State {parent.name} doesn't belong to {SM.name}");
        var state = new Returner();

        callback.Invoke(parent, state, name);
        //StateMachine.BindState(parent, state, name);
        state.SetFieldValue("sm", SM);
        return state;
    }
}
