using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Arguments.MultiplayerArg;
using MultiplayerMod.Network.Common.Interfaces;

namespace MultiplayerMod.Events.Handlers;

public static class MultiplayerEvents
{

    public static event OniEventHandler MultiplayerStarted;
    public static event OniEventHandler MultiplayerStop;
    public static event OniEventHandler ConnectionLost;
    public static event OniEventHandler PlayersReady;
    public static event OniEventHandlerTEventArgs<JoinRequestedArg> JoinRequested;
    public static event OniEventHandlerTEventArgs<ClientInitializationRequestArg> ClientInitializationRequest;

    public static void OnMultiplayerStarted()
    {
        MultiplayerStarted?.Invoke();
    }

    public static void OnMultiplayerStop()
    {
        MultiplayerStop?.Invoke();
    }

    public static void OnConnectionLost()
    {
        ConnectionLost?.Invoke();
    }

    public static void OnPlayersReady()
    {
        PlayersReady?.Invoke();
    }

    public static void OnJoinRequested(IEndPoint endpoint, string hostName)
    {
        JoinRequested?.Invoke(new(endpoint, hostName));
    }

    public static void OnClientInitializationRequest(INetId clientId, PlayerProfile profile)
    {
        ClientInitializationRequest?.Invoke(new(clientId, profile));
    }
}
