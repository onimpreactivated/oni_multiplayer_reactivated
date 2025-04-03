using MultiplayerMod.Core.Serialization;
using MultiplayerMod.Network.Common.Message;
using System.Runtime.InteropServices;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Real steam networking Handle.
/// </summary>
public class SteamNetworkMessageHandle : ISteamNetworkMessageHandle
{
    private GCHandle handle;
    private byte[] bytes;
    /// <inheritdoc/>
    public IntPtr Pointer { get; }

    /// <inheritdoc/>
    public uint Size { get; }

    /// <inheritdoc/>
    public byte[] Message => bytes;


    /// <summary>
    /// Creating new <see cref="SteamNetworkMessageHandle"/>
    /// </summary>
    /// <param name="pointer"></param>
    /// <param name="size"></param>
    public SteamNetworkMessageHandle(IntPtr pointer, uint size)
    {
        Pointer = pointer;
        Size = size;
        bytes = new byte[(int) Size];
        Marshal.Copy(Pointer, bytes, 0, (int) Size);
    }

    /// <summary>
    /// Creating new <see cref="SteamNetworkMessageHandle"/>
    /// </summary>
    /// <param name="msghandle"></param>
    public SteamNetworkMessageHandle(INetworkMessageHandle msghandle)
    {
        bytes = msghandle.Message.Take((int) msghandle.Size).ToArray();
        handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        Pointer = handle.AddrOfPinnedObject();
        Size = (uint) bytes.Length;
    }

    /// <summary>
    /// Creating new <see cref="SteamNetworkMessageHandle"/>
    /// </summary>
    /// <param name="message"></param>
    public SteamNetworkMessageHandle(INetworkMessage message)
    {
        bytes = CoreSerializer.Serialize(message);
        handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        Pointer = handle.AddrOfPinnedObject();
        Size = (uint) bytes.Length;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        handle.Free();
        bytes = null;
    }

}
