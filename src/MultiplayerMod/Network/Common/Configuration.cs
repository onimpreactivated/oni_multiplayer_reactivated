using MultiplayerMod.Network.Common.Message;

namespace MultiplayerMod.Network.Common;

/// <summary>
/// Basic network Configuration
/// </summary>
public static class Configuration
{
    /// <summary>
    /// Max Message Size the Platform can deliver
    /// </summary>
    public const int MaxMessageSize = 524288; // 512 KiB

    /// <summary>
    /// Max Size of the Fragment that it can send.
    /// </summary>
    public static readonly int MaxFragmentDataSize = GetFragmentDataSize();

    private static int GetFragmentDataSize()
    {
        using var serialized = SerializedNetworkMessageHandle.Serialize(new NetworkMessageFragment(0, []));
        return MaxMessageSize - (int) serialized.Size;
    }

    /// <summary>
    /// Currently disabled
    /// </summary>
    public static bool UseLan => false;
}
