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
public class ClickUserMenuButtonCommand : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="GameObject"/>
    /// </summary>
    public GameObjectResolver Resolver { get; }

    /// <summary>
    /// The Method Declaring Type
    /// </summary>
    public Type ActionDeclaringType { get; }

    /// <summary>
    /// The Method Name
    /// </summary>
    public string ActionName { get; }

    public ClickUserMenuButtonCommand(GameObject gameObject, System.Action action)
    {
        Resolver = gameObject.GetGOResolver();
        ActionDeclaringType = action.Method.DeclaringType;
        ActionName = action.Method.Name;
    }
}
