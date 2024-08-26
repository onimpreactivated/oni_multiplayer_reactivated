using System;
using MultiplayerMod.Multiplayer.StateMachines.Configuration;
using MultiplayerMod.Multiplayer.StateMachines.Configuration.Configurers;

namespace MultiplayerMod.Multiplayer.Chores.Synchronizers;

public abstract class ChoreSynchronizer<TChore, TStateMachine, TStateMachineInstance>
    : ChoreSynchronizer<TChore, TStateMachine, TStateMachineInstance, object>
    where TStateMachine : GameStateMachine<TStateMachine, TStateMachineInstance, TChore, object>
    where TStateMachineInstance : GameStateMachine<TStateMachine, TStateMachineInstance, TChore, object>.GameInstance
    where TChore : Chore, IStateMachineTarget;

public abstract class ChoreSynchronizer<TChore, TStateMachine, TStateMachineInstance, TDef>()
    : StateMachineConfigurer(typeof(TChore), typeof(TStateMachine)), IChoreConfigurer
    where TStateMachine : GameStateMachine<TStateMachine, TStateMachineInstance, TChore, TDef>
    where TStateMachineInstance : GameStateMachine<TStateMachine, TStateMachineInstance, TChore, TDef>.GameInstance
    where TChore : Chore, IStateMachineTarget {

    public Type ChoreType => MasterType;

    protected abstract void Configure(IStateMachineRootConfigurer<TStateMachine, TStateMachineInstance, TChore, TDef> root);

    public override void Configure(StateMachineConfigurationContext context) {
        var configurer = new StateMachineConfigurerDsl<TStateMachine, TStateMachineInstance, TChore, TDef>(Configure);
        configurer.Configure(context);
    }

}
