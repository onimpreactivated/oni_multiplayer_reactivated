using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(PauseScreen))]
internal static class PauseScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PauseScreen.TriggerQuitGame))]
    private static void TriggerQuitGame()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (!MultiplayerManager.IsMultiplayer())
            return;
        EventManager.TriggerEvent(new GameQuitEvent());
    }
}
