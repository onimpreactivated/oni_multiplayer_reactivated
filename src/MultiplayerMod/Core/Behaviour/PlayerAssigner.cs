using MultiplayerMod.Core.Player;
using UnityEngine;

namespace MultiplayerMod.Core.Behaviour;

/// <summary>
/// Assing a custom player to this obejct
/// </summary>
public class PlayerAssigner : MonoBehaviour
{
    /// <summary>
    /// Assigned <see cref="CorePlayer"/> to <see cref="GameObject"/>
    /// </summary>
    public CorePlayer Player { get; set; }
}
