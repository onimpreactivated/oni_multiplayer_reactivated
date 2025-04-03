using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(TemperatureSwitchSideScreen))]
internal static class TemperatureSwitchSideScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(TemperatureSwitchSideScreen.OnTargetTemperatureChanged))]
    private static void OnTargetTemperatureChanged(TemperatureSwitchSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(TemperatureSwitchSideScreen.OnConditionButtonClicked))]
    private static void OnConditionButtonClicked(TemperatureSwitchSideScreen __instance) => TriggerEvent(__instance);

    private static void TriggerEvent(TemperatureSwitchSideScreen instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new UpdateTemperatureSwitchCommand(
            instance.targetTemperatureSwitch.GetComponentResolver(),
            instance.targetTemperatureSwitch.thresholdTemperature,
            instance.targetTemperatureSwitch.activateOnWarmerThan
            ));
    }
}
