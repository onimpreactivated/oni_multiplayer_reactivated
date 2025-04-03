using MultiplayerMod.Core;
using MultiplayerMod.Core.Unity;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Steam related starter
/// </summary>
public class SteamNetwork
{
    /// <summary>
    /// Initialize platform related actions.
    /// </summary>
    public static void Init()
    {
        UnityObjectManager.CreateStaticWithComponent<SteamLobbyJoinRequestComponent>();
        MultiplayerManager.Instance.MultiplayerOperations = new SteamOperation();
        SteamLobby steamLobby = new();
        MultiplayerManager.Instance.NetServer = new SteamNetServer(steamLobby);
        MultiplayerManager.Instance.NetClient = new SteamNetClient(steamLobby);
        MultiplayerManager.Instance.PlayerProfileProvider = new SteamPlayerProfileProvider();
        Debug.Log("SteamNetwork Init Finished");
    }
}
