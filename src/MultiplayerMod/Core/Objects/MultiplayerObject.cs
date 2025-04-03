namespace MultiplayerMod.Core.Objects;

/// <summary>
/// Repersentation of a Unique object in Multiplayer
/// </summary>
/// <param name="id">Identification</param>
/// <param name="generation">Number of generation is is cycled through</param>
/// <param name="persistent">Is object must exist</param>
public class MultiplayerObject(MultiplayerId id, int generation, bool persistent = true)
{
    /// <summary>
    /// The Object's Identification
    /// </summary>
    public MultiplayerId Id => id;

    /// <summary>
    /// Is object must exist
    /// </summary>
    public bool Persistent => persistent;

    /// <summary>
    /// Number of generation is is cycled through
    /// </summary>
    public int Generation => generation;

    /// <summary>
    /// Returns a value indicating whether this instance and a specified <see cref="MultiplayerObject"/> object represent the same value.
    /// </summary>
    /// <param name="other">An object to compare to this instance.</param>
    /// <returns>true if <paramref name="other"/> is equal to this instance; otherwise, false.</returns>
    protected bool Equals(MultiplayerObject other) => Id.Equals(other.Id);

    /// <summary>
    /// Returns a value indicating whether this instance and a specified <see cref="MultiplayerObject"/> object represent the same value.
    /// </summary>
    /// <param name="other">An object to compare to this instance.</param>
    /// <returns>true if <paramref name="other"/> is equal to this instance; otherwise, false.</returns>
    public override bool Equals(object other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return other.GetType() == GetType() && Equals((MultiplayerObject) other);
    }

    /// <summary>
    /// Get The HashCode of <see cref="Id"/>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Checks if <paramref name="left"/> and <paramref name="right"/> are <see cref="object.Equals(object, object)"/>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(MultiplayerObject left, MultiplayerObject right) => Equals(left, right);

    /// <summary>
    /// Checks if <paramref name="left"/> and <paramref name="right"/> are not <see cref="object.Equals(object, object)"/>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(MultiplayerObject left, MultiplayerObject right) => !Equals(left, right);

    /// <summary>
    /// Returnig as [P] if <see cref="Persistent"/> and the <see cref="Id"/>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{(Persistent ? "[P] " : "")}{Id}";

}
