using System.Collections.ObjectModel;

namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Command that checks the server DLC's.
/// </summary>
/// <param name="dlcList"></param>
[Serializable]
public class DLC_CheckCommand(List<string> dlcList) : BaseCommandEvent
{
    /// <summary>
    /// DLC list what the client own. (And installed)
    /// </summary>
    public ReadOnlyCollection<string> ClientDLCs => dlcList.AsReadOnly();
}

/// <summary>
/// Command result if user compatible with the dls.
/// </summary>
/// <param name="isOk"></param>
[Serializable]
public class DLC_ResultCommand(bool isOk) : BaseCommandEvent
{
    /// <summary>
    /// Client can play.
    /// </summary>
    public bool IsOk => isOk;
}
