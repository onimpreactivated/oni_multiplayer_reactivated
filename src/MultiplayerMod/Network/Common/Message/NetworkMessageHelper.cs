using MultiplayerMod.Commands.NetCommands;
using static MultiplayerMod.Network.Common.Configuration;

namespace MultiplayerMod.Network.Common.Message;

/// <summary>
/// Static helper for <see cref="BaseCommandEvent"/>
/// </summary>
public static class NetworkMessageHelper
{
    /// <summary>
    /// Creates a new <see cref="IEnumerable{T}"/> of <see cref="INetworkMessageHandle"/> (Many are <see cref="SerializedNetworkMessageHandle"/> )
    /// </summary>
    /// <param name="command"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IEnumerable<INetworkMessageHandle> Create(BaseCommandEvent command, MultiplayerCommandOptions options)
    {
        return Create(new NetworkMessage(command, options));
    }

    internal static IEnumerable<INetworkMessageHandle> Create(NetworkMessage networkMessage)
    {
        using var message = SerializedNetworkMessageHandle.Serialize(networkMessage);
        if (message.Size <= MaxMessageSize)
        {
            yield return message;
            yield break;
        }

        var fragmentsCount = (int) message.Size / MaxFragmentDataSize + 1;
        var header = new NetworkMessageFragmentsHeader(fragmentsCount);
        var serializedHeader = SerializedNetworkMessageHandle.Serialize(header);
        yield return serializedHeader;

        // TODO: Do this better
        for (var i = 0; i < fragmentsCount; i++)
        {
            var offset = i * MaxFragmentDataSize;
            var bufferSize = Math.Min(Math.Max((int) message.Size - offset, 0), MaxFragmentDataSize);

            var data = new byte[bufferSize];
            Buffer.BlockCopy(message.Message, offset, data, 0, bufferSize);
            using var serialized = SerializedNetworkMessageHandle.Serialize(new NetworkMessageFragment(header.MessageId, data));
            yield return serialized;
        }
    }
}
