namespace MultiplayerMod.Core.Player;

/// <summary>
/// Repersentation of a Player
/// </summary>
[Serializable]
public class CorePlayer(PlayerRole role, PlayerProfile profile)
{
    /// <summary>
    /// The Player's Identification
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// The Player's State
    /// </summary>
    public PlayerState State { get; set; } = PlayerState.Initializing;

    /// <summary>
    /// The Player's Role
    /// </summary>
    public PlayerRole Role => role;

    /// <summary>
    /// The Player's profile
    /// </summary>
    public PlayerProfile Profile => profile;

    /// <summary>
    /// Returns a value indicating whether this instance and a specified <see cref="CorePlayer"/> object represent the same value.
    /// </summary>
    /// <param name="other">An object to compare to this instance.</param>
    /// <returns>true if <paramref name="other"/> is equal to this instance; otherwise, false.</returns>
    protected bool Equals(CorePlayer other) => Id.Equals(other.Id);

    /// <summary>
    /// Returns a value indicating whether this instance and a specified <see cref="CorePlayer"/> object represent the same value.
    /// </summary>
    /// <param name="other">An object to compare to this instance.</param>
    /// <returns>true if <paramref name="other"/> is equal to this instance; otherwise, false.</returns>
    public override bool Equals(object other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return other is CorePlayer player && Equals(player);
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
    public static bool operator ==(CorePlayer left, CorePlayer right) => Equals(left, right);

    /// <summary>
    /// Checks if <paramref name="left"/> and <paramref name="right"/> are not <see cref="object.Equals(object, object)"/>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(CorePlayer left, CorePlayer right) => !Equals(left, right);

    /// <summary>
    /// Returnig as "<see cref="CorePlayer"/> {{ Id = <see cref="Id"/> }}"
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(CorePlayer)} {{ Id = {Id} }}";
}
