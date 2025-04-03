using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Exceptions;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Events.Others;
using MultiplayerMod.Network.Common;
using MultiplayerMod.Network.Common.Interfaces;

namespace MultiplayerMod.Multiplayer.EventCalls;

/// <summary>
/// Mulitplayer Server related Event calls
/// </summary>
public class MPServerCalls
{
    internal static readonly Dictionary<INetId, Guid> identities = [];

    /// <summary>
    /// Registering <see cref="INetServer"/> Events
    /// </summary>
    public static void Registers()
    {
        MultiplayerManager.Instance.NetServer.ClientDisconnected += OnClientDisconnected;
        MultiplayerManager.Instance.NetServer.StateChanged += OnServerStateChanged;
    }

    internal static void OnServerStateChanged(NetStateServer state)
    {
        if (state == NetStateServer.Started)
            MultiplayerManager.Instance.NetClient.Connect(MultiplayerManager.Instance.NetServer.Endpoint);
    }

    internal static void OnClientDisconnected(INetId id)
    {
        if (!identities.TryGetValue(id, out var playerId))
            throw new PlayersManagementException($"No associated player found for client {id}");

        var player = MultiplayerManager.Instance.MultiGame.Players[playerId];
        MultiplayerManager.Instance.NetServer.Send(new RemovePlayerCommand(player.Id, "Client Disconnected"));
        identities.Remove(id);
        Debug.Log($"Client {id} disconnected {{ Id = {player.Id} }}");
    }

    internal static void OnClientInitializationRequested(ClientInitializationRequestEvent @event)
    {
        var host = @event.ClientId.Equals(MultiplayerManager.Instance.NetClient.Id);
        var role = host ? PlayerRole.Server : PlayerRole.Client;
        var player = new CorePlayer(role, @event.Profile);
        if (identities.ContainsKey(@event.ClientId))
            identities[@event.ClientId] = player.Id;
        else
            identities.Add(@event.ClientId, player.Id);

        MultiplayerManager.Instance.NetServer.Send(@event.ClientId, new SyncPlayersCommand([.. MultiplayerManager.Instance.MultiGame.Players]));
        MultiplayerManager.Instance.NetServer.Clients.ForEach(it => MultiplayerManager.Instance.NetServer.Send(it, new AddPlayerCommand(player, it.Equals(@event.ClientId))));
        Debug.Log($"Client {@event.ClientId} initialized {{ Role = {role}, Id = {player.Id} }}");
    }

    [UnsubAfterCall]
    internal static void ResumeGameOnReady(PlayersReadyEvent _)
    {
        // we currently skip this one here.
        /*
        if (!@event.IsPaused)
            MultiplayerManager.Instance.NetServer.Send(new ResumeGameCommand());
        */
    }
}
