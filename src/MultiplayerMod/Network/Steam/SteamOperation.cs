using MultiplayerMod.Network.Common.Interfaces;
using Steamworks;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Steam related <see cref="IMultiplayerOperations"/>
/// </summary>
public class SteamOperation : IMultiplayerOperations
{
    /// <inheritdoc/>
    public void Join() => SteamFriends.ActivateGameOverlay("friends");
}
