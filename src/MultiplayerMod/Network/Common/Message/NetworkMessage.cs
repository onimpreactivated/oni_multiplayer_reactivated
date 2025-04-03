using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Serialization;

namespace MultiplayerMod.Network.Common.Message;

/// <summary>
/// Basic implementation of <see cref="INetworkMessage"/>
/// </summary>
[Serializable]
public class NetworkMessage(BaseCommandEvent @event, MultiplayerCommandOptions options) : INetworkMessage
{
    /// <summary>
    /// <see cref="BaseCommandEvent"/>
    /// </summary>
    public BaseCommandEvent CommandEvent => @event;

    /// <summary>
    /// <see cref="MultiplayerCommandOptions"/>
    /// </summary>
    public MultiplayerCommandOptions Options => options;

    /// <summary>
    /// Serialize the current <see cref="NetworkMessage"/> to <see cref="Array"/> of <see cref="byte"/>
    /// </summary>
    /// <returns></returns>
    public byte[] ToBytes()
    {
        return CoreSerializer.Serialize(this);
    }

    /// <summary>
    /// Converting <paramref name="rawData"/> to <see cref="NetworkMessage"/>
    /// </summary>
    /// <param name="rawData">Converted <see cref="NetworkMessage"/></param>
    /// <returns><see cref="NetworkMessage"/></returns>
    public static NetworkMessage ParseFrom(byte[] rawData)
    {
        return CoreSerializer.Deserialize<NetworkMessage>(rawData);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[NM] CE:{CommandEvent} OP:{Options}";
    }
}
