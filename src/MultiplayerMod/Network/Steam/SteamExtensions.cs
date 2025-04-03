using MultiplayerMod.Network.Common.Message;
using Steamworks;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Steam related Extenstions
/// </summary>
public static class SteamExtensions
{
    /// <summary>
    /// Creates a new <see cref="SteamNetworkMessageHandle"/> from <paramref name="message"/>
    /// </summary>
    /// <param name="message"></param>
    /// <returns><see cref="SteamNetworkMessageHandle"/></returns>
    public static INetworkMessageHandle GetNetworkMessageHandle(this SteamNetworkingMessage_t message) =>
        new SteamNetworkMessageHandle(message.m_pData, (uint) message.m_cbSize);
}
