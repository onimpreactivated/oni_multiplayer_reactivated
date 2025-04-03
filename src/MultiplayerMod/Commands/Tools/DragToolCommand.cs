using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// 
/// </summary>
[Serializable]
public class DragToolCommand(DragCompleteEventArgs args, Type dragToolType) : BaseCommandEvent
{
    /// <summary>
    /// Argument for <see cref="DragTool"/>
    /// </summary>
    public DragCompleteEventArgs Args => args;

    /// <summary>
    /// Type of this DragToolCommand
    /// </summary>
    public Type DragToolType => dragToolType;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{base.ToString()} Args: {Args} Type: {DragToolType}";
    }
}
