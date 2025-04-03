using MultiplayerMod.Core.Context;

namespace MultiplayerMod.Commands.Tools.Args;

/// <summary>
/// Modify using <see cref="DebugTool"/>
/// </summary>
/// <param name="dragEventArgs"></param>
/// <param name="type"></param>
/// <param name="toolContext"></param>
[Serializable]
public class ModifyEventArgs(
    DragCompleteEventArgs dragEventArgs,
    DebugTool.Type type,
    DebugToolContext toolContext)
{
    /// <summary>
    /// Argument for <see cref="DragTool"/>
    /// </summary>
    public DragCompleteEventArgs DragEventArgs => dragEventArgs;

    /// <summary>
    /// The <see cref="DebugTool.Type"/>
    /// </summary>
    public DebugTool.Type Type => type;

    /// <summary>
    /// Contect for <see cref="DebugToolContext"/>
    /// </summary>
    public DebugToolContext ToolContext => toolContext;
}
