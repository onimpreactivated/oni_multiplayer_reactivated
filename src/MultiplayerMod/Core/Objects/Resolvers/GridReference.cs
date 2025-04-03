using MultiplayerMod.Core.Behaviour;
using UnityEngine;

namespace MultiplayerMod.Core.Objects.Resolvers;

/// <summary>
/// Creates a Grid Reference for <see cref="GameObject"/>
/// </summary>
[Serializable]
public class GridReference : GameObjectResolver
{
    /// <summary>
    /// Call where Game Object is
    /// </summary>
    public int Cell { get; }

    /// <summary>
    /// Layer presentation of Object.
    /// </summary>
    public int Layer { get; }

    /// <summary>
    /// Creates a new <see cref="GridReference"/>
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="layer"></param>
    public GridReference(int cell, int layer)
    {
        Cell = cell;
        Layer = layer;
    }

    /// <summary>
    /// Creates a new <see cref="GridReference"/>
    /// </summary>
    /// <param name="gameObject"></param>
    public GridReference(GameObject gameObject)
    {
        var extension = gameObject.GetComponent<GridObject>();
        Cell = Grid.PosToCell(gameObject);
        Layer = extension != null ? extension.GridLayer : 0;
    }

    /// <inheritdoc/>
    protected override GameObject ResolveGameObject() => Grid.Objects[Cell, Layer];

    /// <inheritdoc/>
    public override string ToString() => $"{{ Cell = {Cell}, Layer = {Layer} }}";

    /// <inheritdoc/>
    protected bool Equals(GridReference other) => Cell == other.Cell && Layer == other.Layer;

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((GridReference) obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => Cell * 397 ^ Layer;
}
