using UnityEngine;

namespace MultiplayerMod.Events.Common;

/// <summary>
/// Event that 
/// </summary>
/// <param name="gameObject"></param>
public class GameObjectCreated(GameObject gameObject) : BaseEvent
{
    /// <summary>
    /// Created Game Object
    /// </summary>
    public GameObject GameObject => gameObject;
}
