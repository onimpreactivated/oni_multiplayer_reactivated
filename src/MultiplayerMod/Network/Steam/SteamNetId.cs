using MultiplayerMod.Network.Common.Interfaces;
using Steamworks;

namespace MultiplayerMod.Network.Steam;

/// <summary>
/// Steam repressentation of unique Id
/// </summary>
/// <param name="id">The steamId</param>
public class SteamNetId(CSteamID id) : INetId
{
    /// <summary>
    /// Identification of <see cref="INetId"/>
    /// </summary>
    public CSteamID Id => id;

    /// <inheritdoc/>
    public bool Equals(INetId other)
    {
        return other is SteamNetId player && player.Id == this.Id;
    }

    /// <inheritdoc/>
    public bool Equals(INetId x, INetId y)
    {
        if (x is not SteamNetId x_si)
            return false;
        if (y is not SteamNetId y_si)
            return false;
        return x_si.Id == y_si.Id;
    }

    /// <inheritdoc/>
    public int GetHashCode(INetId obj)
    {
        return ((SteamNetId) obj).Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"(SteamNetId {Id})";
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is INetId id && this.Equals(id);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
