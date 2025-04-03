namespace MultiplayerMod.Core.Player;

/// <summary>
/// Interface to many platform that provide <see cref="PlayerProfile"/>
/// </summary>
public interface IPlayerProfileProvider
{
    /// <summary>
    /// Get the Platform related <see cref="PlayerProfile"/> info.
    /// </summary>
    /// <returns>Filled <see cref="PlayerProfile"/></returns>
    PlayerProfile GetPlayerProfile();
}
