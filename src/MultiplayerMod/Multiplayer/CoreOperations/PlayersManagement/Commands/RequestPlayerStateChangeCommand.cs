﻿using System;
using MultiplayerMod.Multiplayer.Commands;
using MultiplayerMod.Multiplayer.Players;
using MultiplayerMod.Network;

namespace MultiplayerMod.Multiplayer.CoreOperations.PlayersManagement.Commands;

[Serializable]
[MultiplayerCommand(Type = MultiplayerCommandType.System, ExecuteOnServer = true)]
public class RequestPlayerStateChangeCommand : MultiplayerCommand {

    private PlayerIdentity playerId;
    private PlayerState state;

    public RequestPlayerStateChangeCommand(PlayerIdentity playerId, PlayerState state) {
        this.playerId = playerId;
        this.state = state;
    }

    public override void Execute(MultiplayerCommandContext context) {
        var server = context.Runtime.Dependencies.Get<IMultiplayerServer>();
        server.SendAll(new ChangePlayerStateCommand(playerId, state));
    }

}
