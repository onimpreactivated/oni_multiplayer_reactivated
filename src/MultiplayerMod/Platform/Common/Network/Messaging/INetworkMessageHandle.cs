using System;

namespace MultiplayerMod.Platform.Common.Network.Messaging;

public interface INetworkMessageHandle : IDisposable
{
    public IntPtr Pointer { get; }
    public uint Size { get; }
}
