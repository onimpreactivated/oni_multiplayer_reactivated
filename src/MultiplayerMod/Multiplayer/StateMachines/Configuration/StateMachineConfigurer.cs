using System;
using MultiplayerMod.Multiplayer.StateMachines.Configuration.Configurers;

namespace MultiplayerMod.Multiplayer.StateMachines.Configuration;

public interface IStateMachineConfigurer {
    Type MasterType { get; }
    Type StateMachineType { get; }

    void Configure(StateMachineConfigurationContext context);
}

public abstract class StateMachineConfigurer(Type masterType, Type stateMachineType) : IStateMachineConfigurer {
    public Type MasterType { get; } = masterType;
    public Type StateMachineType { get; } = stateMachineType;

    public abstract void Configure(StateMachineConfigurationContext context);
}

public abstract class StateMachineConfigurer<TStateMachine, TStateMachineInstance, TMaster, TDef>()
    : StateMachineConfigurer(typeof(TMaster), typeof(TStateMachine))
    where TStateMachine : GameStateMachine<TStateMachine, TStateMachineInstance, TMaster, TDef>
    where TStateMachineInstance : GameStateMachine<TStateMachine, TStateMachineInstance, TMaster, TDef>.GameInstance
    where TMaster : IStateMachineTarget {

    protected abstract void Configure(IStateMachineRootConfigurer<TStateMachine, TStateMachineInstance, TMaster, TDef> root);

    public override void Configure(StateMachineConfigurationContext context) {
        var configurer = new StateMachineConfigurerDsl<TStateMachine, TStateMachineInstance, TMaster, TDef>(Configure);
        configurer.Configure(context);
    }

}
