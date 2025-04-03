using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;
using UnityEngine;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Synchronize the object to the position
/// </summary>
[Serializable]
public class SynchronizeObjectPositionCommand : BaseCommandEvent
{
    /// <summary>
    /// Synchronize the <paramref name="gameObject"/> to its position.
    /// </summary>
    /// <param name="gameObject"></param>
    public SynchronizeObjectPositionCommand(GameObject gameObject)
    {
        Resolver = gameObject.GetGOResolver();
        Position = gameObject.transform.GetPosition();
        var facing = gameObject.GetComponent<Facing>();
        if (facing != null)
            FacingLeft = facing.facingLeft;
    }

    /// <summary>
    /// Resolver for <see cref="GameObject"/>
    /// </summary>
    public GameObjectResolver Resolver { get; }

    /// <summary>
    /// Position to Synchronize
    /// </summary>
    public Vector3 Position { get; }

    /// <summary>
    /// Should face to the left
    /// </summary>
    public bool? FacingLeft { get; }
}
