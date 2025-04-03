using UnityEngine;

namespace MultiplayerMod.Core.Objects.Resolvers;

/// <summary>
/// Reference that creates a <see cref="MultiplayerId"/>
/// </summary>
/// <param name="id"></param>
[Serializable]
public class MultiplayerIdReference(MultiplayerId id) : GameObjectResolver
{

    /// <summary>
    /// A multiplayer Id for this <see cref="GameObject"/>
    /// </summary>
    public MultiplayerId Id => id;

    /// <inheritdoc/>
    protected override GameObject ResolveGameObject() => MultiplayerManager.Instance.MPObjects.Get<GameObject>(Id);

    /// <inheritdoc/>
    public override string ToString() => Id.ToString();

    /// <inheritdoc/>
    protected bool Equals(MultiplayerIdReference other) => Id.Equals(other.Id);

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((MultiplayerIdReference) obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}
