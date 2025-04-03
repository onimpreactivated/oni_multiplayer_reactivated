using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;
using UnityEngine;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// 
/// </summary>
/// <param name="gameObject"></param>
/// <param name="action"></param>
[Serializable]
public class ClickUserMenuButtonCommand(GameObject gameObject, System.Action action) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="GameObject"/>
    /// </summary>
    public GameObjectResolver Resolver => gameObject.GetGOResolver();

    /// <summary>
    /// The Method Declaring Type
    /// </summary>
    public Type ActionDeclaringType => action.Method.DeclaringType!;

    /// <summary>
    /// The Method Name
    /// </summary>
    public string ActionName => action.Method.Name;
}
