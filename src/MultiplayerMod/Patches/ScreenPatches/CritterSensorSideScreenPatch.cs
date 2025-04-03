using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(CritterSensorSideScreen))]
internal class CritterSensorSideScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CritterSensorSideScreen.ToggleCritters))]
    internal static void ToggleCritters(CritterSensorSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(CritterSensorSideScreen.ToggleEggs))]
    internal static void ToggleEggs(CritterSensorSideScreen __instance) => TriggerEvent(__instance);

    internal static void TriggerEvent(CritterSensorSideScreen instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new UpdateCritterSensorCommand(
            instance.targetSensor.GetComponentResolver(),
            instance.targetSensor.countCritters,
            instance.targetSensor.countEggs
        ));
    }
}
