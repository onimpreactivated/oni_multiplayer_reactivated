namespace MultiplayerMod.Multiplayer.Datas.World;

/// <summary>
/// Creates a new WorldSave for multiplayer
/// </summary>
/// <param name="name"></param>
/// <param name="saveData"></param>
/// <param name="state"></param>
[Serializable]
public class WorldSave(string name, byte[] saveData, WorldState state)
{
    /// <summary>
    /// The world Name
    /// </summary>
    public string Name => name;

    /// <summary>
    /// The world save data
    /// </summary>
    public byte[] Data => saveData;

    /// <summary>
    /// Custom state for the world
    /// </summary>
    public WorldState State => state;
}
