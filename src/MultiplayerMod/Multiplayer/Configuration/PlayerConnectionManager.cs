﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MultiplayerMod.Core.Events;
using MultiplayerMod.Core.Logging;
using MultiplayerMod.Multiplayer.CoreOperations.Events;
using MultiplayerMod.Multiplayer.Players;
using MultiplayerMod.Multiplayer.Players.Commands;
using MultiplayerMod.Multiplayer.Players.Events;
using MultiplayerMod.Multiplayer.World;
using MultiplayerMod.Network;

namespace MultiplayerMod.Multiplayer.Configuration;

[UsedImplicitly]
public class PlayerConnectionManager {

    private readonly Core.Logging.Logger log = LoggerFactory.GetLogger<PlayerConnectionManager>();

    private readonly IMultiplayerServer server;
    private readonly IMultiplayerClient client;

    private readonly IPlayerProfileProvider profileProvider;
    private readonly WorldManager worldManager;
    private readonly MultiplayerGame multiplayer;

    private readonly Dictionary<IMultiplayerClientId, PlayerIdentity> identities = new();

    public PlayerConnectionManager(
        IMultiplayerServer server,
        IMultiplayerClient client,
        IPlayerProfileProvider profileProvider,
        WorldManager worldManager,
        EventDispatcher events,
        MultiplayerGame multiplayer
    ) {
        this.server = server;
        this.client = client;
        this.profileProvider = profileProvider;
        this.worldManager = worldManager;
        this.multiplayer = multiplayer;

        client.StateChanged += OnClientStateChanged;
        server.ClientDisconnected += ServerOnClientDisconnected;
        events.Subscribe<ClientInitializationRequestEvent>(OnClientInitializationRequested);
        events.Subscribe<GameQuitEvent>(OnGameQuit);
        events.Subscribe<GameStartedEvent>(OnGameStarted);
    }

    private void OnGameQuit(GameQuitEvent _) {
        client.Send(new RequestPlayerStateChangeCommand(multiplayer.Players.Current.Id, PlayerState.Leaving));
        client.Disconnect();
    }

    private void ServerOnClientDisconnected(IMultiplayerClientId clientId) {
        if (!identities.TryGetValue(clientId, out var playerId))
            throw new MultiplayerConnectionException($"No associated player found for client {clientId}");

        var player = multiplayer.Players[playerId];
        server.Send(new RemovePlayerCommand(player.Id));
        identities.Remove(clientId);
        log.Debug($"Client {client} disconnected");
    }

    private void OnClientStateChanged(MultiplayerClientState state) {
        switch (state) {
            case MultiplayerClientState.Connected:
                client.Send(
                    new InitializeClientCommand(profileProvider.GetPlayerProfile()),
                    MultiplayerCommandOptions.ExecuteOnServer
                );
                break;
            case MultiplayerClientState.Disconnected:
                multiplayer.Players.Synchronize(Array.Empty<MultiplayerPlayer>());
                break;
        }
    }

    private void OnGameStarted(GameStartedEvent @event) {
        if (@event.Multiplayer.Mode != MultiplayerMode.Client)
            return;

        var currentPlayer = multiplayer.Players.Current;
        var command = new RequestPlayerStateChangeCommand(currentPlayer.Id, PlayerState.Ready);
        client.Send(command, MultiplayerCommandOptions.ExecuteOnServer);
    }

    private void OnClientInitializationRequested(ClientInitializationRequestEvent @event) {
        var host = @event.ClientId.Equals(client.Id);
        var role = host ? PlayerRole.Host : PlayerRole.Client;
        var player = new MultiplayerPlayer(role, @event.Profile);
        identities[@event.ClientId] = player.Id;

        server.Send(@event.ClientId, new SyncPlayersCommand(multiplayer.Players.ToArray()));
        server.Clients.ForEach(
            clientId => server.Send(clientId, new AddPlayerCommand(player, clientId.Equals(@event.ClientId)))
        );

        if (host) {
            log.Debug($"Host {@event.ClientId} initialized");
            server.Send(new ChangePlayerStateCommand(player.Id, PlayerState.Ready));
            return;
        }

        log.Debug($"Client {@event.ClientId} initialized");
        worldManager.Sync();
    }

}
