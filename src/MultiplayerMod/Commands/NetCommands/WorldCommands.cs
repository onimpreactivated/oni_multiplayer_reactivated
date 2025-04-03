using MultiplayerMod.Multiplayer.Datas.World;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Loading world save
/// </summary>
/// <param name="world"></param>
[Serializable]
public class LoadWorldCommand(WorldSave world) : BaseCommandEvent
{
    /// <summary>
    /// The WorldSave
    /// </summary>
    public WorldSave World => world;
}
