using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Events.Others;
using Steamworks;
using UnityEngine;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Component that handles Joining to new players via command line
/// </summary>
public class SteamLobbyJoinRequestComponent : MonoBehaviour
{
    private readonly Callback<GameLobbyJoinRequested_t> lobbyJoinRequestedCallback;

    /// <summary>
    /// Creates the component
    /// </summary>
    public SteamLobbyJoinRequestComponent()
    {
        lobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
        EventManager.SubscribeEvent<MainMenuInitialized>(OnMainMenuInitialized);
    }

    [UnsubAfterCall]
    private void OnMainMenuInitialized(MainMenuInitialized main)
    {
        ProcessCommandLine();
    }

    private void ProcessCommandLine()
    {
        var arguments = Environment.GetCommandLineArgs();
        if (!arguments.Contains("+connect_lobby"))
            return;
        int index = arguments.Where(arg => arg.Contains("+connect_lobby")).Select((arg, ind) => ind).FirstOrDefault();
        string lobby_id = arguments.ElementAtOrDefault(index + 1);

        if (string.IsNullOrEmpty(lobby_id))
            return;

        var id = new CSteamID(ulong.Parse(lobby_id));
        Debug.Log($"Requesting connection to lobby {id} (command line)");
        DispatchJoinRequest(id);
    }

    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t request)
    {
        Debug.Log($"Requesting connection to lobby {request.m_steamIDLobby} (lobby join request)");
        DispatchJoinRequest(request.m_steamIDLobby);
    }

    private void DispatchJoinRequest(CSteamID lobbyId)
    {
        var endpoint = new SteamServerEndpoint(lobbyId);
        var dataUpdateCallback = new Callback<LobbyDataUpdate_t>[1];
        dataUpdateCallback[0] = Callback<LobbyDataUpdate_t>.Create(_ =>
        {
            var serverName = SteamMatchmaking.GetLobbyData(lobbyId, "server.name");
            dataUpdateCallback[0].Unregister();
            EventManager.TriggerEvent(new MultiplayerJoinRequestedEvent(endpoint, serverName));
        });
        SteamMatchmaking.RequestLobbyData(lobbyId);
    }

    internal void OnDestroy()
    {
        lobbyJoinRequestedCallback.Unregister();
    }
}
