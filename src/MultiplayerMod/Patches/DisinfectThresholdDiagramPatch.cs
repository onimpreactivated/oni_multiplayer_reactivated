using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(DisinfectThresholdDiagram))]
internal static class DisinfectThresholdDiagramPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(DisinfectThresholdDiagram.OnReleaseHandle))]
    internal static void OnReleaseHandle() => TriggerEvent();

    [HarmonyPostfix]
    [HarmonyPatch(nameof(DisinfectThresholdDiagram.ReceiveValueFromSlider))]
    internal static void ReceiveValueFromSlider() => TriggerEvent();

    [HarmonyPostfix]
    [HarmonyPatch(nameof(DisinfectThresholdDiagram.ReceiveValueFromInput))]
    internal static void ReceiveValueFromInput() => TriggerEvent();

    [HarmonyPostfix]
    [HarmonyPatch(nameof(DisinfectThresholdDiagram.OnClickToggle))]
    internal static void OnClickToggle() => TriggerEvent();

    internal static void TriggerEvent()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new SetDisinfectSettingsCommand(SaveGame.Instance.minGermCountForDisinfect, SaveGame.Instance.enableAutoDisinfect));
    }
}
