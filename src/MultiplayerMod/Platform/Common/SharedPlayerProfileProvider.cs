using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.Players;
using MultiplayerMod.Platform.Lan;
using MultiplayerMod.Platform.Steam;

namespace MultiplayerMod.Platform.Common;

[Dependency, UsedImplicitly]
public class SharedPlayerProfileProvider : IPlayerProfileProvider {

    public PlayerProfile GetPlayerProfile() => instance.GetPlayerProfile();

    private IPlayerProfileProvider? cachedInstance = null;
    private IPlayerProfileProvider instance {
        get {
            if (cachedInstance != null) { return cachedInstance; }

            if (Configuration.useSteam) {
                cachedInstance = new SteamPlayerProfileProvider();
            } else {
                cachedInstance = new LanPlayerProfileProvider();
            }
            return cachedInstance;
        }
    }

}
