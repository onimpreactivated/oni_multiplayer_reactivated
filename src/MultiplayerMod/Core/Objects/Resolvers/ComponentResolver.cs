using UnityEngine;

namespace MultiplayerMod.Core.Objects.Resolvers;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Any <see cref="Component"/></typeparam>
/// <param name="reference"></param>
[Serializable]
public class ComponentResolver<T>(GameObjectResolver reference) : TypedResolver<T> where T : Component
{
    private GameObjectResolver GameObjectReference => reference;
    private Type ComponentType => typeof(T);

    /// <inheritdoc/>
    public override T Resolve() => (T) GameObjectReference.Resolve().GetComponent(ComponentType);

    /// <summary>
    /// Returns a value indicating whether this instance and a specified <see cref="ComponentResolver"/> object represent the same value.
    /// </summary>
    /// <param name="other">An object to compare to this instance.</param>
    /// <returns>true if <paramref name="other"/> is equal to this instance; otherwise, false.</returns>
    protected bool Equals(ComponentResolver other)
    {
        return GameObjectReference.Equals(other.GameObjectReference) && ComponentType == other.ComponentType;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((ComponentResolver) obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => GameObjectReference.GetHashCode() * 397 ^ ComponentType.GetHashCode();

}

/// <summary>
/// 
/// </summary>
/// <param name="reference"></param>
/// <param name="type"></param>
[Serializable]
public class ComponentResolver(GameObjectResolver reference, Type type) : ComponentResolver<Component>(reference)
{
    private readonly Type type = type;

    /// <inheritdoc/>
    public override Component Resolve() => base.Resolve().GetComponent(type);
}

