using System;
using System.Collections.Generic;
using MultiplayerMod.Multiplayer.Commands;

namespace MultiplayerMod.Network;

public interface IMultiplayerServer {
    void Start();
    void Stop();

    MultiplayerServerState State { get; }
    IMultiplayerEndpoint Endpoint { get; }
    List<IMultiplayerClientId> Clients { get; }

    void Send(IMultiplayerClientId clientId, IMultiplayerCommand command);

    // TODO: Temporary solution for simplification.
    // TODO: Should be extracted into the upper "players" layer later.
    // Send a command to all clients, including host
    void SendAll(IMultiplayerCommand command);

    // Send a command to all clients
    void Send(IMultiplayerCommand command);

    event Action<MultiplayerServerState> StateChanged;
    event Action<IMultiplayerClientId> ClientConnected;
    event Action<IMultiplayerClientId> ClientDisconnected;
    event Action<IMultiplayerClientId, IMultiplayerCommand> CommandReceived;
}
