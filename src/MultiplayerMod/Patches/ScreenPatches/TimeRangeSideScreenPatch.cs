using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(TimeRangeSideScreen))]
internal static class TimeRangeSideScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(TimeRangeSideScreen.ChangeSetting))]
    internal static void ChangeSetting(TimeRangeSideScreen __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new UpdateLogicTimeOfDaySensorCommand(
            __instance.targetTimedSwitch.GetComponentResolver(),
            __instance.targetTimedSwitch.startTime,
            __instance.targetTimedSwitch.duration
        ));
    }
}
