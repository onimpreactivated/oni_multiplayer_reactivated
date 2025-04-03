using MultiplayerMod.Network.Common.Interfaces;
using UnityEngine;

namespace MultiplayerMod.Network.Common.Components;

/// <summary>
/// Component for Server
/// </summary>
public class ServerComponent : MonoBehaviour
{
    /// <summary>
    /// The Server
    /// </summary>
    public INetServer server;

    internal void Update() => server?.Tick();
}
