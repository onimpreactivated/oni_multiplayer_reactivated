using MultiplayerMod.Core.Behaviour;

namespace MultiplayerMod.Extensions;

/// <summary>
/// Extension class for <see cref="MinionIdentity"/>
/// </summary>
public static class MinionIdentityExtensions
{
    /// <summary>
    /// Getting the <see cref="MultiplayerInstance"/> from <paramref name="identity"/>
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    public static MultiplayerInstance GetMultiplayerInstance(this MinionIdentity identity)
    {
        identity.ValidateProxy();
        var proxy = identity.assignableProxy.Get();
        return proxy.GetComponent<MultiplayerInstance>();
    }
}
