using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer;
using MultiplayerMod.Platform.Lan;
using MultiplayerMod.Platform.Steam;

namespace MultiplayerMod.Platform.Common;

[Dependency, UsedImplicitly]
public class SharedMultiplayerOperations : IMultiplayerOperations {

    public void Join() => instance.Join();

    private IMultiplayerOperations? cachedInstance = null;
    private IMultiplayerOperations instance {
        get {
            if (cachedInstance != null) { return cachedInstance; }

            if (Configuration.useSteam) {
                cachedInstance = new SteamMultiplayerOperations();
            } else {
                cachedInstance = new LanMultiplayerOperations();
            }
            return cachedInstance;
        }
    }

}
