using UnityEngine;

namespace MultiplayerMod.Commands.Tools.Args;

/// <summary>
/// Argument for <see cref="DragTool.OnDragComplete(Vector3, Vector3)"/>
/// </summary>
/// <param name="cells"></param>
/// <param name="cursorDown"></param>
/// <param name="cursorUp"></param>
/// <param name="priority"></param>
/// <param name="parameters"></param>
[Serializable]
public class DragCompleteEventArgs(
    List<int> cells,
    Vector3 cursorDown,
    Vector3 cursorUp,
    PrioritySetting priority,
    string[] parameters)
{
    /// <summary>
    /// Cells selected
    /// </summary>
    public List<int> Cells => cells;

    /// <summary>
    /// Cursor Down position
    /// </summary>
    public Vector3 CursorDown => cursorDown;

    /// <summary>
    /// Cursor up position
    /// </summary>
    public Vector3 CursorUp => cursorUp;

    /// <summary>
    /// Priority for the <see cref="DragTool"/>
    /// </summary>
    public PrioritySetting Priority => priority;

    /// <summary>
    /// Parameters for the <see cref="DragTool"/>
    /// </summary>
    public string[] Parameters => parameters;
}
