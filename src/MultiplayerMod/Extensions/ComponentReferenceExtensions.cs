using MultiplayerMod.Core.Objects.Resolvers;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Extensions;

/// <summary>
/// Extensions for getting <see cref="ComponentResolver"/>
/// </summary>
public static class ComponentReferenceExtensions
{
    /// <summary>
    /// Getting <see cref="ComponentResolver"/> from <paramref name="component"/>
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ComponentResolver GetComponentResolver(this KMonoBehaviour component) => new(
        component.gameObject.GetGOResolver(),
        component.GetType()
    );

    /// <summary>
    /// Get <see cref="ComponentResolver{T}"/> from <paramref name="component"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ComponentResolver<T> GetComponentResolver<T>(this T component) where T : KMonoBehaviour =>
        new(component.gameObject.GetGOResolver());
}
