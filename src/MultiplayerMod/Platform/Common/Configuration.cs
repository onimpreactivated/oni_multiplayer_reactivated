using MultiplayerMod.Platform.Common.Network.Messaging;
using MultiplayerMod.Platform.Lan.Network;
using System;

namespace MultiplayerMod.Platform.Common;

public static class Configuration
{
    public const int MaxMessageSize = 524288; // 512 KiB
    public static readonly int MaxFragmentDataSize = GetFragmentDataSize();

    private static int GetFragmentDataSize() {
        using var serialized = NetworkSerializer.Serialize(new NetworkMessageFragment(0, Array.Empty<byte>()));
        return MaxMessageSize - (int) serialized.Size;
    }

    public static bool useSteam { get { return !LanConfiguration.instance.isConfigured; } }
}
