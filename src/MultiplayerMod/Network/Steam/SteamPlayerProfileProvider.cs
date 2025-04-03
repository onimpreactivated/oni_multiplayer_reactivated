using MultiplayerMod.Core.Player;
using Steamworks;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Creates a Steam Player Profile Provider with userName
/// </summary>
public class SteamPlayerProfileProvider : IPlayerProfileProvider
{
    /// <inheritdoc/>
    public PlayerProfile GetPlayerProfile() => new(SteamFriends.GetPersonaName());
}
