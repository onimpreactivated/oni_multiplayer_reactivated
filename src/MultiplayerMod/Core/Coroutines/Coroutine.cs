namespace EIV_Common.Coroutines;

/// <summary>
/// Represent a Coroutine
/// </summary>
/// <param name="enumerator"></param>
/// <param name="type"></param>
/// <param name="tag"></param>
public struct Coroutine(IEnumerator<double> enumerator, CoroutineType type, string tag = "")
    : IEquatable<Coroutine>, IEqualityComparer<Coroutine>
{
    /// <summary>
    /// Getting the currnt Time
    /// </summary>
    public IEnumerator<double> Enumerator = enumerator;
    /// <summary>
    /// Is Coroutine currently running
    /// </summary>
    public bool IsRunning;
    /// <summary>
    /// Should Kill this coroutine
    /// </summary>
    public bool ShouldKill;
    /// <summary>
    /// Should pause this coroutine
    /// </summary>
    public bool ShouldPause;
    /// <summary>
    /// Is Coroutine successly run 
    /// </summary>
    public bool IsSuccess;
    /// <summary>
    /// Tag to unique names
    /// </summary>
    public readonly string Tag = tag;
    /// <summary>
    /// Type of Coroutine
    /// </summary>
    public readonly CoroutineType CoroutineType = type;

    private readonly IEnumerator<double> _baseEnumerator = enumerator;

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return _baseEnumerator != null ? _baseEnumerator.GetHashCode() : 0;
    }

    /// <inheritdoc/>
    public bool Equals(Coroutine other)
    {
        return this.GetHashCode() == other.GetHashCode();
    }

    /// <inheritdoc/>
    public bool Equals(Coroutine x, Coroutine y)
    {
        return x.GetHashCode() == y.GetHashCode();
    }

    /// <inheritdoc/>
    public int GetHashCode(Coroutine obj)
    {
        return obj.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{this.GetHashCode()} IsRunning: {IsRunning}, ShouldKill {ShouldKill}, ShouldPause: {ShouldPause}, IsSuccess: {IsSuccess}, CoroutineType: {CoroutineType} Tag: {Tag}";
    }
}
