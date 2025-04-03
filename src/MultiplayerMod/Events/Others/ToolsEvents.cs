using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Events.Others;

/// <summary>
/// Event called when <see cref="DragTool.OnDragComplete(UnityEngine.Vector3, UnityEngine.Vector3)"/> called 
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public class DragCompletedEvent(object sender, DragCompleteEventArgs args) : BaseEvent
{
    /// <summary>
    /// Type who sent the <see cref="DragTool.OnDragComplete(UnityEngine.Vector3, UnityEngine.Vector3)"/>
    /// </summary>
    public object Sender => sender;

    /// <summary>
    /// Argument for <see cref="DragTool"/>
    /// </summary>
    public DragCompleteEventArgs Args => args;
}
