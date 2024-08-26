using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.StateMachines.Configuration;
using MultiplayerMod.Multiplayer.StateMachines.Configuration.Configurers;

namespace MultiplayerMod.Multiplayer.Chores.Synchronizers;

[Dependency, UsedImplicitly]
public class AttackChoreSynchronizer : ChoreSynchronizer<AttackChore, AttackChore.States, AttackChore.StatesInstance> {

    protected override void Configure(IStateMachineRootConfigurer<AttackChore.States, AttackChore.StatesInstance, AttackChore, object> root) {
        root.Inline(new StateMachineConfigurerDsl<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>(monitor => {
            monitor.PreConfigure(MultiplayerMode.Client, pre => {
                pre.Suppress(() => pre.StateMachine.threatened.duplicant.ShouldFight.ToggleChore(null, null));
            });
        }));
    }

}
