using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(TimerSideScreen))]
internal static class TimerSideScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(TimerSideScreen.ChangeSetting))]
    internal static void ChangeSetting(TimerSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(TimerSideScreen.ToggleMode))]
    internal static void UpdateMaxCapacity(TimerSideScreen __instance) => TriggerEvent(__instance);

    internal static void TriggerEvent(TimerSideScreen instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new UpdateLogicTimeSensorCommand(
            instance.targetTimedSwitch.GetComponentResolver(),
            instance.targetTimedSwitch.displayCyclesMode,
            instance.targetTimedSwitch.onDuration,
            instance.targetTimedSwitch.offDuration,
            instance.targetTimedSwitch.timeElapsedInCurrentState
        ));
    }
}
