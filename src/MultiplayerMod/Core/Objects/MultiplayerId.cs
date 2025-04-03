using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Objects;

/// <summary>
/// Identification for <see cref="MultiplayerObject"/>
/// </summary>
[Serializable]
public class MultiplayerId
{
    /// <summary>
    /// <see cref="InternalMultiplayerIdType"/> or <see cref="Guid"/> first 8 bytes
    /// </summary>
    public long HighPart { get; }

    /// <summary>
    /// Internal Id if <see cref="Type"/> is <see cref="MultiplayerIdType.Internal"/> or <see cref="Guid"/> last 8 bytes
    /// </summary>
    public long LowPart { get; }

    /// <summary>
    /// Identification Type
    /// </summary>
    public MultiplayerIdType Type { get; }

    /// <summary>
    /// Creates a new <see cref="MultiplayerId"/>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="internalId"></param>
    public MultiplayerId(InternalMultiplayerIdType type, long internalId)
    {
        Type = MultiplayerIdType.Internal;
        HighPart = (long) type;
        LowPart = internalId;
    }

    /// <summary>
    /// Creates a new <see cref="MultiplayerId"/>
    /// </summary>
    /// <param name="guid"></param>
    public MultiplayerId(Guid guid)
    {
        var bytes = guid.ToByteArray();
        Type = MultiplayerIdType.Generated;
        HighPart = GetLong(bytes, 0);
        LowPart = GetLong(bytes, 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private long GetLong(byte[] bytes, int offset)
    {
        return bytes[offset] |
               (long) bytes[offset + 1] << 8 |
               (long) bytes[offset + 2] << 16 |
               (long) bytes[offset + 3] << 24 |
               (long) bytes[offset + 4] << 32 |
               (long) bytes[offset + 5] << 40 |
               (long) bytes[offset + 6] << 48 |
               (long) bytes[offset + 7] << 56;
    }

    /// <summary>
    /// Returns a value indicating whether this instance and a specified <see cref="MultiplayerId"/> object represent the same value.
    /// </summary>
    /// <param name="other">An object to compare to this instance.</param>
    /// <returns>true if <paramref name="other"/> is equal to this instance; otherwise, false.</returns>
    protected bool Equals(MultiplayerId other)
    {
        return Type == other.Type && HighPart == other.HighPart && LowPart == other.LowPart;
    }

    /// <inheritdoc/>
    public override bool Equals(object other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return other.GetType() == GetType() && Equals((MultiplayerId) other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = HighPart.GetHashCode();
        hashCode = hashCode * 397 ^ LowPart.GetHashCode();
        hashCode = hashCode * 397 ^ Type.GetHashCode();
        return hashCode;
    }


    /// <summary>
    /// Checks if <paramref name="left"/> and <paramref name="right"/> are <see cref="object.Equals(object, object)"/>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(MultiplayerId left, MultiplayerId right) => Equals(left, right);

    /// <summary>
    /// Checks if <paramref name="left"/> and <paramref name="right"/> are not <see cref="object.Equals(object, object)"/>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(MultiplayerId left, MultiplayerId right) => !Equals(left, right);

    /// <inheritdoc/>
    public override string ToString() => $"{(byte) Type:x1}:{HighPart:x16}:{LowPart:x16}";

}
