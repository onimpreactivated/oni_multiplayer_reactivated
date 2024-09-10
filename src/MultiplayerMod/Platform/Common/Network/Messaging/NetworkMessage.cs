using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MultiplayerMod.Multiplayer.Commands;
using MultiplayerMod.Network;
using MultiplayerMod.Platform.Common.Network.Messaging.Surrogates;

namespace MultiplayerMod.Platform.Common.Network.Messaging;

[Serializable]
public class NetworkMessage : INetworkMessage
{
    private static BinaryFormatter formatter = new BinaryFormatter() { SurrogateSelector = SerializationSurrogates.Selector };

    public IMultiplayerCommand Command { get; }
    public MultiplayerCommandOptions Options { get; }

    public NetworkMessage(IMultiplayerCommand command, MultiplayerCommandOptions options)
    {
        Command = command;
        Options = options;
    }

    public byte[] toBytes() {
        using (var memoryStream = new MemoryStream()) {
            formatter.Serialize(memoryStream, this);
            byte[] data = new byte[memoryStream.Length];
            Array.Copy(memoryStream.GetBuffer(), 0, data, 0, data.Length);
            return data;
        }
    }

    public static NetworkMessage from(byte[] rawData) {
        using (var memoryStream = new MemoryStream(rawData)) {
            var message = (NetworkMessage) formatter.Deserialize(memoryStream);
            return message;
        }
    }
}
