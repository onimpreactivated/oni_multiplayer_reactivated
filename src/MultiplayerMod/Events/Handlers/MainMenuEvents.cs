using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Arguments.Common;

namespace MultiplayerMod.Events.Handlers;

public static class MainMenuEvents
{
    public static OniEventHandler MainMenuInitialized;
    public static OniEventHandler SinglePlayerModeSelected;
    public static OniEventHandlerTEventArgs<PlayerRoleArg> MultiplayerModeSelected;

    public static void OnMainMenuInitialized()
    {
        MainMenuInitialized?.Invoke();
    }

    public static void OnSinglePlayerModeSelected()
    {
        SinglePlayerModeSelected?.Invoke();
    }

    public static void OnMultiplayerModeSelected(PlayerRole role)
    {
        MultiplayerModeSelected?.Invoke(new(role));
    }
}
