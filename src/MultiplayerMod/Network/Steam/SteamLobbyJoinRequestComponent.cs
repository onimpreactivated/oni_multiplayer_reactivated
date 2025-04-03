using MultiplayerMod.Events.Handlers;
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
        MainMenuEvents.MainMenuInitialized += OnMainMenuInitialized;
    }

    private void OnMainMenuInitialized()
    {
        ProcessCommandLine();
        MainMenuEvents.MainMenuInitialized -= OnMainMenuInitialized;
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
            MultiplayerEvents.OnJoinRequested(endpoint, serverName);
        });
        SteamMatchmaking.RequestLobbyData(lobbyId);
    }

    internal void OnDestroy()
    {
        lobbyJoinRequestedCallback.Unregister();
    }
}
