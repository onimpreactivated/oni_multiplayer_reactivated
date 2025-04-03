namespace MultiplayerMod.Network.Common.Message;

/// <summary>
/// Common Network Message Handle
/// </summary>
public interface INetworkMessageHandle : IDisposable
{
    /// <summary>
    /// Message as <see cref="Array"/> of <see cref="byte"/>
    /// </summary>
    public byte[] Message { get; }

    /// <summary>
    /// Size of the <see cref="Message"/>
    /// </summary>
    public uint Size { get; }
}

/// <summary>
/// Steam related Networking Message Handle
/// </summary>
public interface ISteamNetworkMessageHandle : INetworkMessageHandle
{
    /// <summary>
    /// Pointer for the <see cref="INetworkMessageHandle.Message"/>
    /// </summary>
    public IntPtr Pointer { get; }
}

/// <summary>
/// Epic related Networking Message Handle (Not Added Yet)
/// </summary>
public interface IEpicNetworkMessageHandle : INetworkMessageHandle;
