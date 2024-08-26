using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.StateMachines.Configuration;
using MultiplayerMod.Multiplayer.StateMachines.Configuration.Configurers;

namespace MultiplayerMod.Multiplayer.Chores.Synchronizers;

[Dependency, UsedImplicitly]
public class PeeChoreSynchronizer : ChoreSynchronizer<PeeChore, PeeChore.States, PeeChore.StatesInstance> {

    protected override void Configure(IStateMachineRootConfigurer<PeeChore.States, PeeChore.StatesInstance, PeeChore, object> root) {
        root.Inline(new StateMachineConfigurerDsl<PeeChoreMonitor, PeeChoreMonitor.Instance>(monitor => {
            monitor.PreConfigure(MultiplayerMode.Client, pre => pre.Suppress(() => pre.StateMachine.pee.ToggleChore(null, null)));
        }));
    }

}
