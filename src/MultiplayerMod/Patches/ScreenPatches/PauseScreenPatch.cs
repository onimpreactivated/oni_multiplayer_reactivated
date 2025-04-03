using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events.Handlers;

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
        GameEvents.OnGameQuit();
    }
}
