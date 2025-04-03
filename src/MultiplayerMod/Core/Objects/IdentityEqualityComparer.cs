using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Objects;

/// <summary>
/// Comparing the <typeparamref name="T"/> with <see cref="object.ReferenceEquals"/> and <see cref="RuntimeHelpers.GetHashCode"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class IdentityEqualityComparer<T> : IEqualityComparer<T>
{
    /// <inheritdoc/>
    public bool Equals(T x, T y) => ReferenceEquals(x, y);

    /// <inheritdoc/>
    public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode(obj);
}
