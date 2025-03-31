using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Network.Common;
using UnityEngine;

namespace MultiplayerMod.ChoreSync.Syncs
{
    internal class IdleChoreSync : BaseChoreSync<IdleChore.States>
    {
        public override Type SyncType => typeof(IdleChore);

        public override void Server(StateMachine instance)
        {
            Setup(instance);

            SM.idle.move.Enter(smi =>
            {
                Debug.Log("[IdleChoreSync] Server: Enter idle.move");

                int cell = smi.GetIdleCell();
                var resolver = new ChoreStateMachineResolver(smi.master);

                var netServer = MultiplayerManager.Instance.NetServer;
                netServer.Send(new MoveObjectToCellCommand(resolver, cell, SM.idle.move), MultiplayerCommandOptions.SkipHost);
            });

            SM.idle.move.Exit(smi =>
            {
                Debug.Log("[IdleChoreSync] Server: Exit idle.move");

                var resolver = new ChoreStateMachineResolver(smi.master);
                var netServer = MultiplayerManager.Instance.NetServer;

                netServer.Send(new GoToStateCommand(resolver, SM.idle), MultiplayerCommandOptions.SkipHost);
                netServer.Send(new SynchronizeObjectPositionCommand(smi.gameObject), MultiplayerCommandOptions.SkipHost);
            });
        }

        public override void Client(StateMachine instance)
        {
            Setup(instance);

            Debug.Log("[IdleChoreSync] Client: Synchronizing idle state.");

            // Multiplayer-Parameter für Zielzelle hinzufügen
            var targetCell = AddMultiplayerParameter<int, IdleChore.States.IntParameter>(MoveObjectToCellCommand.TargetCell);

            // MoveTo-Funktion mit Multiplayer-Parameter für Zielzelle
            SM.idle.move.MoveTo(targetCell.Get, SM.idle, SM.idle);
        }
    }
}
