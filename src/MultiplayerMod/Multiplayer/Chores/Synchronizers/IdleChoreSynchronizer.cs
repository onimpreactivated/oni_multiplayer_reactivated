using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.Commands.Objects;
using MultiplayerMod.Multiplayer.Objects.Reference;
using MultiplayerMod.Multiplayer.StateMachines.Commands;
using MultiplayerMod.Multiplayer.StateMachines.Configuration;
using MultiplayerMod.Multiplayer.StateMachines.Configuration.Configurers;
using MultiplayerMod.Network;

namespace MultiplayerMod.Multiplayer.Chores.Synchronizers;

[Dependency, UsedImplicitly]
public class IdleChoreSynchronizer(IMultiplayerServer server) : ChoreSynchronizer<IdleChore, IdleChore.States, IdleChore.StatesInstance> {

    protected override void Configure(IStateMachineRootConfigurer<IdleChore.States, IdleChore.StatesInstance, IdleChore, object> root) {
        // Disable IdleChore recurring creation
        root.Inline(new StateMachineConfigurerDsl<IdleMonitor, IdleMonitor.Instance>(monitor => {
            monitor.PreConfigure(MultiplayerMode.Client, pre => {
                pre.Suppress(() => pre.StateMachine.idle.ToggleRecurringChore(null, null));
            });
        }));

        root.PreConfigure(MultiplayerMode.Host, SetupHost);
        root.PreConfigure(MultiplayerMode.Client, SetupClient);
    }

    private void SetupClient(StateMachinePreConfigurer<IdleChore.States, IdleChore.StatesInstance, IdleChore, object> configurer) {
        var sm = configurer.StateMachine;

        // Suppress any methods that result in transition to "idle.move" state
        configurer.Suppress(() => sm.idle.onfloor.ToggleScheduleCallback("", null, null));
        configurer.Suppress(() => sm.idle.onladder.ToggleScheduleCallback("", null, null));
        configurer.Suppress(() => sm.idle.ontube.Update("", null, 0, false));
        configurer.Suppress(() => sm.idle.onsuitmarker.ToggleScheduleCallback("", null, null));

        // Suppress "idle.move" transitions and move bindings
        configurer.Suppress(() => sm.idle.move.Transition(null, null, 0));
        configurer.Suppress(() => sm.idle.move.MoveTo(null, null, null, false));

        // Configure "idle.move" to move to the synchronized cell
        configurer.PostConfigure(post => {
            var targetCell = post.AddMultiplayerParameter(MoveObjectToCell.TargetCell);
            sm.idle.move.MoveTo(smi => targetCell.Get(smi), sm.idle, sm.idle);
        });
    }

    private void SetupHost(StateMachinePreConfigurer<IdleChore.States, IdleChore.StatesInstance, IdleChore, object> configurer) {
        var sm = configurer.StateMachine;

        sm.idle.move.Enter(smi => {
            var cell = smi.GetIdleCell();
            server.Send(new MoveObjectToCell(new ChoreStateMachineReference(smi.master), cell, sm.idle.move));
        });

        sm.idle.move.Exit(smi => {
            server.Send(new GoToState(new ChoreStateMachineReference(smi.master), sm.idle));
            server.Send(new SynchronizeObjectPosition(smi.gameObject));
        });
    }

}
