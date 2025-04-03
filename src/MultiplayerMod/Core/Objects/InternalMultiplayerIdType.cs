namespace MultiplayerMod.Core.Objects;

/// <summary>
/// Identify Type of <see cref="MultiplayerIdType"/> if its <see cref="MultiplayerIdType.Internal"/>
/// </summary>
public enum InternalMultiplayerIdType : long
{
    /// <summary>
    /// Id created by Prefab
    /// </summary>
    KPrefabId = 0,
    /// <summary>
    /// Id created by Chore
    /// </summary>
    Chore = 1
}
