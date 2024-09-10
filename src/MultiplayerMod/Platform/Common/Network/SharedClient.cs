using System;
using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.Commands;
using MultiplayerMod.Network;
using MultiplayerMod.Platform.Steam.Network;
using MultiplayerMod.Platform.Lan.Network;

namespace MultiplayerMod.Platform.Common.Network;

[Dependency, UsedImplicitly]
public class SharedClient : IMultiplayerClient
{
    public MultiplayerClientState State { get { return client.State; } }

    public event Action<MultiplayerClientState>? StateChanged { add { client.StateChanged += value; } remove { client.StateChanged -= value; } }
    public event Action<IMultiplayerCommand>? CommandReceived { add { client.CommandReceived += value; } remove { client.CommandReceived -= value; } }
    public IMultiplayerClientId Id { get { return client.Id; } }

    public void Connect(IMultiplayerEndpoint endpoint) { client.Connect(endpoint); }
    public void Disconnect() { client.Disconnect(); }
    public void Send(IMultiplayerCommand command, MultiplayerCommandOptions options = MultiplayerCommandOptions.None) { client.Send(command, options); }
    public void Tick() { client.Tick(); }

    private IMultiplayerClient? cachedClient = null;
    private IMultiplayerClient client
    {
        get
        {
            if (cachedClient != null) { return cachedClient; }

            if (Configuration.useSteam) {
                cachedClient = new SteamClient();
            } else {
                cachedClient = new LanClient();
            }
            return cachedClient;
        }
    }

}
