namespace EIV_Common.Coroutines;

/// <summary>
/// Handle for a coroutine.
/// </summary>
public struct CoroutineHandle(int hash, CoroutineType type) : IEquatable<CoroutineHandle>, IEqualityComparer<CoroutineHandle>
{
    /// <summary>
    /// Hash of the Coroutine
    /// </summary>
    public int CoroutineHash => hash;
    /// <summary>
    /// Type of the Corootuine
    /// </summary>
    public CoroutineType CoroutineType => type;

    /// <inheritdoc/>
    public bool Equals(CoroutineHandle other)
    {
        return CoroutineHash == other.CoroutineHash;
    }

    /// <inheritdoc/>
    public bool Equals(CoroutineHandle x, CoroutineHandle y)
    {
        return x.CoroutineHash == y.CoroutineHash;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return CoroutineHash;
    }

    /// <inheritdoc/>
    public int GetHashCode(CoroutineHandle obj)
    {
        return obj.CoroutineHash;
    }

    /// <inheritdoc/>
    public static implicit operator int(CoroutineHandle coroutineHandle)
    {
        return coroutineHandle.CoroutineHash;
    }

    /// <inheritdoc/>
    public static implicit operator CoroutineHandle(Coroutine coroutine)
    {
        return new CoroutineHandle(coroutine.GetHashCode(), coroutine.CoroutineType);
    }
}
