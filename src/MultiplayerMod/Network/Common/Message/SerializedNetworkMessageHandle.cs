using MultiplayerMod.Core.Serialization;

namespace MultiplayerMod.Network.Common.Message;

/// <summary>
/// Serialized <see cref="INetworkMessage"/>
/// </summary>
/// <remarks>
/// Creates a new <see cref="SerializedNetworkMessageHandle"/> with the serialized <see cref="INetworkMessage"/>
/// </remarks>
/// <param name="message">The <see cref="INetworkMessage"/></param>
public class SerializedNetworkMessageHandle(INetworkMessage message) : INetworkMessageHandle
{
    private byte[] message = CoreSerializer.Serialize(message);
    /// <inheritdoc/>
    public byte[] Message => message;

    /// <inheritdoc/>
    public uint Size => (uint) message.Length;

    /// <summary>
    /// Serializing any <see cref="INetworkMessage"/> to <see cref="SerializedNetworkMessageHandle"/>
    /// </summary>
    /// <param name="message">The <see cref="INetworkMessage"/></param>
    /// <returns><see cref="SerializedNetworkMessageHandle"/></returns>
    public static SerializedNetworkMessageHandle Serialize(INetworkMessage message)
    {
        return new SerializedNetworkMessageHandle(message);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.message = null;
    }
}
