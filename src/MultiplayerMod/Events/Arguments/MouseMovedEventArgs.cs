using UnityEngine;

namespace MultiplayerMod.Events.Arguments;

/// <summary>
/// Arguments that is used to update the mouse position
/// </summary>
/// <param name="position"></param>
/// <param name="positionWithinScreen"></param>
/// <param name="screenName"></param>
/// <param name="screenType"></param>
[Serializable]
public class MouseMovedEventArgs(Vector2 position, Vector2? positionWithinScreen, string screenName, string screenType)
{
    /// <summary>
    /// Position of the mouse
    /// </summary>
    public Vector2 Position => position;

    /// <summary>
    /// Position of the mouse inside the screen
    /// </summary>
    public Vector2? PositionWithinScreen => positionWithinScreen;

    /// <summary>
    /// The name of the screen
    /// </summary>
    public string ScreenName => screenName;

    /// <summary>
    /// The type of the screen
    /// </summary>
    public string ScreenTypeName => screenType;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Pos: {Position.ToString()} PosWithScreen: {(PositionWithinScreen == null ? "null" : PositionWithinScreen.ToString())}, ScreenName: {ScreenName}, ScreenType: {ScreenTypeName}";
    }
}
