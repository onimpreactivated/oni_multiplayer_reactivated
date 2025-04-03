namespace MultiplayerMod.Extensions;

/// <summary>
/// Simple extension for <see cref="MainMenu"/>
/// </summary>
public static class MainMenuExtensions
{
    /// <summary>
    /// Add button to MainMenu.
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="text"></param>
    /// <param name="highlight"></param>
    /// <param name="action"></param>
    public static void AddButton(this MainMenu menu, string text, bool highlight, System.Action action)
    {
        menu.MakeButton(new MainMenu.ButtonInfo(
            new LocString(text),
            action,
            20,
            highlight ? menu.topButtonStyle : menu.normalButtonStyle
        ));
    }
}
