using UnityEngine;

namespace MultiplayerMod.Events.Arguments.Common;

public class GameObjectArg(GameObject value) : EventArgs
{
    public GameObject Value { get; } = value;
}