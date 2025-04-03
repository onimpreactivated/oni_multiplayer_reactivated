using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Events.MainMenu;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(MainMenu))]
internal static class MainMenuPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenu.OnPrefabInit))]
    internal static void OnPrefabInit(MainMenu __instance)
    {
        __instance.AddButton(
            "NEW MULTIPLAYER",
            highlight: false,
            () => UseMultiplayerMode(PlayerRole.Server, __instance.NewGame)
        );
        __instance.AddButton(
            "LOAD MULTIPLAYER",
            highlight: false,
            () => UseMultiplayerMode(PlayerRole.Server, __instance.LoadGame)
        );

        __instance.AddButton(
            "JOIN MULTIPLAYER",
            highlight: false,
            () => UseMultiplayerMode(PlayerRole.Client, MultiplayerManager.Instance.MultiplayerOperations.Join)
        );
    }
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenu.MakeButton))]
    internal static void MakeButton(ref MainMenu.ButtonInfo info)
    {
        if (!info.Is(STRINGS.UI.FRONTEND.MAINMENU.NEWGAME) && !info.Is(STRINGS.UI.FRONTEND.MAINMENU.LOADGAME))
            return;

        var originalAction = info.action;
        info.action = () => DisableMultiplayer(originalAction);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenu.ResumeGame))]
    internal static void ResumeGame() => DisableMultiplayer();

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MainMenu.OnSpawn))]
    internal static void OnSpawn() => EventManager.TriggerEvent(new MainMenuInitialized());

    internal static bool Is(this MainMenu.ButtonInfo info, LocString loc) => info.text.key.String == loc.key.String;

    internal static void DisableMultiplayer(System.Action action = null)
    {
        EventManager.TriggerEvent(new SinglePlayerModeSelectedEvent());
        action?.Invoke();
    }

    internal static void UseMultiplayerMode(PlayerRole mode, System.Action action)
    {
        MultiplayerManager.Instance.MultiGame.Refresh(mode);
        EventManager.TriggerEvent(new MultiplayerModeSelectedEvent(mode));
        action();
    }
}
