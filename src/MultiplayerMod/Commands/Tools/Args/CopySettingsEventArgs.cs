namespace MultiplayerMod.Commands.Tools.Args;

/// <summary>
/// Argument for <see cref="CopySettingsTool"/>
/// </summary>
/// <param name="dragEvent"></param>
/// <param name="sourceCell"></param>
/// <param name="sourceLayer"></param>
[Serializable]
public class CopySettingsEventArgs(
    DragCompleteEventArgs dragEvent,
    int sourceCell,
    ObjectLayer sourceLayer)
{
    /// <summary>
    /// Base DragComplete Event argument 
    /// </summary>
    public DragCompleteEventArgs DragEvent => dragEvent;

    /// <summary>
    /// Source cell to copy from
    /// </summary>
    public int SourceCell => sourceCell;

    /// <summary>
    /// Layer to copy from
    /// </summary>
    public ObjectLayer SourceLayer => sourceLayer;
}
