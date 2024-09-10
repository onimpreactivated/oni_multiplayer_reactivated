using System;
using MultiplayerMod.Multiplayer.Players;
using Steamworks;

namespace MultiplayerMod.Platform.Steam;

public class SteamPlayerProfileProvider : IPlayerProfileProvider {

    private readonly Lazy<PlayerProfile> profile = new(
        () => new PlayerProfile(
            SteamFriends.GetPersonaName()
        )
    );

    public PlayerProfile GetPlayerProfile() => profile.Value;

}
