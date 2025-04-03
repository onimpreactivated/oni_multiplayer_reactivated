namespace MultiplayerMod.Core.Objects.Resolvers;

/// <summary>
/// Interface that can create <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResolver<T>
{
    /// <summary>
    /// Resolving current class to <typeparamref name="T"/>
    /// </summary>
    /// <returns></returns>
    public abstract T Resolve();
}

/// <summary>
/// Interface that can create <see cref="object"/>
/// </summary>
public interface IResolver
{
    /// <summary>
    /// Resolving current class to <see cref="object"/>
    /// </summary>
    /// <returns></returns>
    public abstract object Resolve();
}

/// <summary>
/// Abstract class that creates <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public abstract class TypedResolver<T> : IResolver<T>, IResolver
{
    /// <inheritdoc/>
    public abstract T Resolve();
    object IResolver.Resolve() => Resolve();
}
