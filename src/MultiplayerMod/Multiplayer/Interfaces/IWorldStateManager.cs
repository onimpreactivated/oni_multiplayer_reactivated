using MultiplayerMod.Multiplayer.Datas.World;

namespace MultiplayerMod.Multiplayer.Interfaces;

/// <summary>
/// State Save and Load implementation
/// </summary>
public interface IWorldStateManager
{
    /// <summary>
    /// Save state to <paramref name="data"/>
    /// </summary>
    /// <param name="data"></param>
    void SaveState(WorldState data);

    /// <summary>
    /// Load states from <paramref name="data"/>
    /// </summary>
    /// <param name="data"></param>
    void LoadState(WorldState data);
}
