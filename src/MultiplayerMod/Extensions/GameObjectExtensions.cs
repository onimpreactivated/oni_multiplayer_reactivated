using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Core.Objects.Resolvers;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MultiplayerMod.Extensions;

/// <summary>
/// 
/// </summary>
public static class GameObjectExtensions
{
    /// <summary>
    /// Getting <see cref="GameObjectResolver"/> from <paramref name="gameObject"/>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GameObjectResolver GetGOResolver(this GameObject gameObject)
    {
        var multiplayerId = gameObject.GetComponent<MultiplayerInstance>().Id;
        if (multiplayerId != null)
            return new MultiplayerIdReference(multiplayerId);

        return new GridReference(gameObject);
    }
}
