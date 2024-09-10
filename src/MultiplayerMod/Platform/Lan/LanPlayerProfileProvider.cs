using MultiplayerMod.Multiplayer.Players;
using MultiplayerMod.Platform.Lan.Network;
using System;

namespace MultiplayerMod.Platform.Lan
{
    internal class LanPlayerProfileProvider : IPlayerProfileProvider {

        private readonly Lazy<PlayerProfile> profile = new(
            () => new PlayerProfile(
                LanConfiguration.instance.playerName
            )
        );

        public PlayerProfile GetPlayerProfile() => profile.Value;
    }
}
