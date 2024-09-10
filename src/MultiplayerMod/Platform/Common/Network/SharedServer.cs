using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.Commands;
using MultiplayerMod.Network;
using MultiplayerMod.Platform.Steam.Network;
using MultiplayerMod.Platform.Lan.Network;

namespace MultiplayerMod.Platform.Common.Network;

[Dependency, UsedImplicitly]
public class SharedServer : IMultiplayerServer
{
    public MultiplayerServerState State { get { return server.State; } }

    public IMultiplayerEndpoint Endpoint { get { return server.Endpoint; } }

    public List<IMultiplayerClientId> Clients { get { return server.Clients; } }

    public event Action<MultiplayerServerState> StateChanged { add { server.StateChanged += value; } remove { server.StateChanged -= value; } }
    public event Action<IMultiplayerClientId> ClientConnected { add { server.ClientConnected += value; } remove { server.ClientConnected -= value; } }
    public event Action<IMultiplayerClientId> ClientDisconnected { add { server.ClientDisconnected += value; } remove { server.ClientDisconnected -= value; } }
    public event Action<IMultiplayerClientId, IMultiplayerCommand> CommandReceived { add { server.CommandReceived += value; } remove { server.CommandReceived -= value; } }

    public void Send(IMultiplayerClientId clientId, IMultiplayerCommand command) { server.Send(clientId, command); }

    public void Send(IMultiplayerCommand command, MultiplayerCommandOptions options = MultiplayerCommandOptions.None) { server.Send(command, options); }

    public void Tick() { server.Tick(); }

    public void Start() { server.Start(); }

    public void Stop() { server.Stop(); }

    private IMultiplayerServer? cachedServer = null;
    private IMultiplayerServer server
    {
        get
        {
            if (cachedServer != null) { return cachedServer; }

            if (Configuration.useSteam) {
                cachedServer = new SteamServer();
            } else {
                cachedServer = new LanServer();
            }
            return cachedServer;
        }
    }

}
