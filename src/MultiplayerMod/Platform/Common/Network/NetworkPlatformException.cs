using System;

namespace MultiplayerMod.Platform.Common.Network;

public class NetworkPlatformException : PlatformException
{
    public NetworkPlatformException(string message) : base(message) { }
    public NetworkPlatformException(string message, Exception cause) : base(message, cause) { }
}
