using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;
using System;
using System.Linq;

namespace MultiplayerMod.ChoreSync
{
    internal abstract class BaseChoreSync<ChoreStateMachine> : IChoreSync
        where ChoreStateMachine : StateMachine
    {
        public Type StateMachineType => typeof(ChoreStateMachine);
        public abstract Type SyncType { get; }

        public ChoreStateMachine SM { get; private set; }

        public abstract void Client(StateMachine instance);
        public abstract void Server(StateMachine instance);

        public void Setup(StateMachine instance)
        {
            if (instance is ChoreStateMachine stateMachine)
            {
                SM = stateMachine;
            }
            else
            {
                throw new InvalidCastException($"Setup fehlgeschlagen: Erwartet {typeof(ChoreStateMachine).Name}, aber erhalten: {instance?.GetType().Name ?? "null"}");
            }
        }

        public Param AddMultiplayerParameter<T, Param>(ParameterInfo<T> parameterInfo)
            where Param : StateMachine.Parameter, new()
        {
            if (parameterInfo.Shared)
            {
                var existingParameter = SM.parameters.FirstOrDefault(it => it.name == parameterInfo.Name);
                if (existingParameter != null)
                    return existingParameter as Param ?? throw new InvalidCastException($"Fehler beim Casten des Parameters {parameterInfo.Name}");
            }

            var parameter = new Param
            {
                name = parameterInfo.Name,
                idx = SM.parameters.Length
            };

            SM.parameters = SM.parameters.Append(parameter).ToArray();
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
            {
                throw new InvalidOperationException($"State {parent.name} gehört nicht zu {SM.name}");
            }

            var state = new Returner();
            callback.Invoke(parent, state, name);
            state.SetFieldValue("sm", SM);

            return state;
        }
    }
}
