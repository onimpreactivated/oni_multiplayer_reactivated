using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(AlarmSideScreen))]
internal static class AlarmSideScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(AlarmSideScreen.OnEndEditName))]
    private static void OnEndEditName(AlarmSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(AlarmSideScreen.OnEndEditTooltip))]
    private static void OnEndEditTooltip(AlarmSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(AlarmSideScreen.TogglePause))]
    private static void TogglePause(AlarmSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(AlarmSideScreen.ToggleZoom))]
    private static void ToggleZoom(AlarmSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(AlarmSideScreen.SelectType))]
    private static void SelectType(AlarmSideScreen __instance) => TriggerEvent(__instance);

    private static void TriggerEvent(AlarmSideScreen instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new UpdateAlarmCommand(new AlarmSideScreenEventArgs(
            instance.targetAlarm.GetComponentResolver(),
            instance.targetAlarm.notificationName,
            instance.targetAlarm.notificationTooltip,
            instance.targetAlarm.pauseOnNotify,
            instance.targetAlarm.zoomOnNotify,
            instance.targetAlarm.notificationType
        )));
    }
}
