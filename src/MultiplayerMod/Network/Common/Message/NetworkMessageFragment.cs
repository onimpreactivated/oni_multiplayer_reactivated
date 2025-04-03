namespace MultiplayerMod.Network.Common.Message;

/// <summary>
/// Networking message that is indicate that is a fragmented message
/// </summary>
/// <remarks>
/// Creates a new fragment
/// </remarks>
/// <param name="messageId"></param>
/// <param name="data"></param>
[Serializable]
public class NetworkMessageFragment(int messageId, byte[] data) : INetworkMessage
{
    /// <summary>
    /// A unique Message Id
    /// </summary>
    public int MessageId { get; } = messageId;

    /// <summary>
    /// Fragmented Data
    /// </summary>
    public byte[] Data { get; } = data;
}
