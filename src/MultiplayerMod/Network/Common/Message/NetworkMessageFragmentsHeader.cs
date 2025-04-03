namespace MultiplayerMod.Network.Common.Message;

/// <summary>
/// Networking message that is indicate that is a fragmented message
/// </summary>
/// <remarks>
/// Creates a new fragment header
/// </remarks>
/// <param name="fragmentsCount"></param>
[Serializable]
public class NetworkMessageFragmentsHeader(int fragmentsCount) : INetworkMessage
{
    /// <summary>
    /// A unique Message Id
    /// </summary>
    public int MessageId { get; } = Interlocked.Increment(ref uniqueMessageId);

    /// <summary>
    /// Count how many fragment exists
    /// </summary>
    public int FragmentsCount { get; } = fragmentsCount;

    private static int uniqueMessageId;
}
