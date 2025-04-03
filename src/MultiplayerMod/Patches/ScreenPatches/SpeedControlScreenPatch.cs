using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events.Handlers;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(SpeedControlScreen))]
internal static class SpeedControlScreenPatch
{
    private static bool eventsEnabled = true;

    internal static bool IsSpeedSetByCommand = false;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(SpeedControlScreen.SetSpeed))]
    private static void SetSpeedPostfix(int Speed)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (!MultiplayerManager.IsMultiplayer())
            return;
        if (!eventsEnabled)
            return;
        if (IsSpeedSetByCommand)
        {
            IsSpeedSetByCommand = false;
            return;
        }
        SpeedControlEvents.OnSpeedControlSetSpeed(Speed);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(SpeedControlScreen.OnPause))]
    private static void OnPausePostfix()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (!MultiplayerManager.IsMultiplayer())
            return;
        if (!eventsEnabled)
        {
            eventsEnabled = true;
            return;
        }
        SpeedControlEvents.OnSpeedControlPause();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(SpeedControlScreen.OnPlay))]
    private static void OnPlayPostfix()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (!MultiplayerManager.IsMultiplayer())
            return;
        if (!eventsEnabled)
            return;
        SpeedControlEvents.OnSpeedControlResume();
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(SpeedControlScreen.DebugStepFrame))]
    private static void DebugStepFramePrefix()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        eventsEnabled = false;
    }
}
