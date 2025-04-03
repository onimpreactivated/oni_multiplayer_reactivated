using MultiplayerMod.Network.Common.Interfaces;
using Steamworks;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Creating steam Lobby Endpoint
/// </summary>
/// <param name="lobbyID"></param>
public class SteamServerEndpoint(CSteamID lobbyID) : IEndPoint
{
    /// <summary>
    /// Steam Lobby EndPoint
    /// </summary>
    public CSteamID LobbyID => lobbyID;
}
