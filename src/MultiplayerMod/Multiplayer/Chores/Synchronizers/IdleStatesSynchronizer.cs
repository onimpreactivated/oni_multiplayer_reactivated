using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.Commands.Objects;
using MultiplayerMod.Multiplayer.Objects;
using MultiplayerMod.Multiplayer.Objects.Extensions;
using MultiplayerMod.Multiplayer.Objects.Reference;
using MultiplayerMod.Multiplayer.StateMachines.Commands;
using MultiplayerMod.Multiplayer.StateMachines.Configuration;
using MultiplayerMod.Multiplayer.StateMachines.Configuration.Configurers;
using MultiplayerMod.Network;

namespace MultiplayerMod.Multiplayer.Chores.Synchronizers;

[Dependency, UsedImplicitly]
public class IdleStatesSynchronizer(
    IMultiplayerServer server,
    MultiplayerObjects objects
) : StateMachineConfigurer<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def> {

    protected override void Configure(IStateMachineRootConfigurer<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def> root) {
        root.PreConfigure(MultiplayerMode.Client, SetupClient);
        root.PostConfigure(MultiplayerMode.Host, SetupHost);
    }

    private void SetupHost(StateMachinePostConfigurer<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def> configurer) {
        var sm = configurer.StateMachine;

        sm.move.Enter(smi => {
            var target = smi.GetComponent<Navigator>().targetLocator;
            var cell = Grid.PosToCell(target);

            // TODO: Remove after critters sync (WorldGenSpawner.Spawnable + new critters)
            if (objects.Get(smi.master.gameObject) == null)
                return;

            var reference = new StateMachineReference(smi.controller.GetReference(), smi.GetType());
            server.Send(new MoveObjectToCell(reference, cell, sm.move));
        });
        sm.move.Exit(smi => {

            // TODO: Remove after critters sync (WorldGenSpawner.Spawnable + new critters)
            if (objects.Get(smi.master.gameObject) == null)
                return;

            var reference = new StateMachineReference(smi.controller.GetReference(), smi.GetType());
            server.Send(new GoToState(reference, sm.loop));
            server.Send(new SynchronizeObjectPosition(smi.gameObject));
        });
    }

    private void SetupClient(StateMachinePreConfigurer<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def> configurer) {
        var sm = configurer.StateMachine;

        // Suppress transitions to the "move" state
        configurer.Suppress(() => sm.loop.ToggleScheduleCallback(null, null, null));

        // Do nothing on move state
        configurer.Suppress(() => sm.move.Enter(null));

        // Configure "move" to move to the synchronized cell
        configurer.PostConfigure(post => {
            var targetCell = post.AddMultiplayerParameter(MoveObjectToCell.TargetCell);
            sm.move.Enter(smi => {
                var navigator = smi.GetComponent<Navigator>();
                navigator.GoTo(targetCell.Get(smi));
            });
        });
    }

}
