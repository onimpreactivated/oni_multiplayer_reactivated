namespace MultiplayerMod.Multiplayer.Datas.World;

/// <summary>
/// Creating a new Custom state for multiplayer
/// </summary>
[Serializable]
public class WorldState
{
    /// <summary>
    /// World Entities
    /// </summary>
    public Dictionary<string, object> Entries { get; } = [];
}
