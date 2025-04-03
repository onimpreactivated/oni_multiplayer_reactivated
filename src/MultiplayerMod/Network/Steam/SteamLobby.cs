using MultiplayerMod.Core.Exceptions;
using Steamworks;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Steam Lobby functions
/// </summary>
public class SteamLobby
{
    /// <summary>
    /// The Steam Lobby Id
    /// </summary>
    public CSteamID Id { get; private set; } = CSteamID.Nil;

    /// <summary>
    /// Checks if connected to Lobby
    /// </summary>
    public bool Connected => Id != CSteamID.Nil;

    /// <summary>
    /// Getting the GameServer Id
    /// </summary>
    public CSteamID GameServerId
    {
        get
        {
            if (!Connected)
                return CSteamID.Nil;

            return !SteamMatchmaking.GetLobbyGameServer(Id, out _, out _, out var serverId) ? CSteamID.Nil : serverId;
        }
        set
        {
            if (Connected)
                SteamMatchmaking.SetLobbyGameServer(Id, 0, 0, value);
        }
    }

    /// <summary>
    /// Events that gets invoked when lobby created
    /// </summary>
    public event System.Action OnCreate;

    /// <summary>
    /// Events that gets invoked when leaving lobby
    /// </summary>
    public event System.Action OnLeave;

    /// <summary>
    /// Events that gets invoked when joining lobby
    /// </summary>
    public event System.Action OnJoin;

    private readonly CallResult<LobbyCreated_t> lobbyCreatedCallback;
    private readonly CallResult<LobbyEnter_t> lobbyEnteredCallback;

    /// <summary>
    /// Creates a new Steam Lobby
    /// </summary>
    public SteamLobby()
    {
        lobbyCreatedCallback = CallResult<LobbyCreated_t>.Create(LobbyCreatedCallback);
        lobbyEnteredCallback = CallResult<LobbyEnter_t>.Create(LobbyEnteredCallback);
    }

    /// <summary>
    /// Creating a new lobby
    /// </summary>
    /// <param name="type"></param>
    /// <param name="maxPlayers"></param>
    public void Create(ELobbyType type = ELobbyType.k_ELobbyTypeFriendsOnly, int maxPlayers = 4)
    {
        if (Id != CSteamID.Nil)
            Leave();
        lobbyCreatedCallback.Set(SteamMatchmaking.CreateLobby(type, maxPlayers));
    }

    /// <summary>
    /// Join to lobby with <paramref name="lobbyId"/>
    /// </summary>
    /// <param name="lobbyId"></param>
    public void Join(CSteamID lobbyId)
    {
        if (Id == lobbyId)
            return;
        if (Id != CSteamID.Nil)
            Leave();
        lobbyEnteredCallback.Set(SteamMatchmaking.JoinLobby(lobbyId));
    }

    /// <summary>
    /// Leaving current lobby
    /// </summary>
    public void Leave()
    {
        if (Id == CSteamID.Nil)
            return;
        SteamMatchmaking.LeaveLobby(Id);
        try
        {
            OnLeave?.Invoke();
        }
        finally
        {
            Id = CSteamID.Nil;
        }
    }

    private void LobbyCreatedCallback(LobbyCreated_t result, bool failure)
    {
        if (failure)
            throw new NetworkPlatformException("I/O failure.");

        if (result.m_eResult != EResult.k_EResultOK)
            throw new NetworkPlatformException($"Unable to create a lobby. Error: {result.m_eResult}");

        Id = new CSteamID(result.m_ulSteamIDLobby);
        Debug.Log($"Lobby {Id} created");
        OnCreate?.Invoke();
    }

    private void LobbyEnteredCallback(LobbyEnter_t result, bool failure)
    {
        if (failure)
            throw new NetworkPlatformException("I/O failure.");

        Id = new CSteamID(result.m_ulSteamIDLobby);
        Debug.Log($"Joined to lobby {Id}");
        OnJoin?.Invoke();
    }
}
