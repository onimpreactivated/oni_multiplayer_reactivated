using Steamworks;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Steam related Configuration
/// </summary>
public static class SteamConfiguration
{
    private const int defaultBufferSize = 10485760; // 10 MiB
    /// <summary>
    /// Creates a new <see cref="SteamNetworkingConfigValue_t"/> that has default buffer size
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static SteamNetworkingConfigValue_t SendBufferSize(int size = defaultBufferSize) => new()
    {
        m_eValue = ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_SendBufferSize,
        m_eDataType = ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32,
        m_val = new SteamNetworkingConfigValue_t.OptionValue { m_int32 = size }
    };
}
