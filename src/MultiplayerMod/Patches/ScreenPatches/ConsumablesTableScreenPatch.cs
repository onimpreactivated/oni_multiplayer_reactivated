using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using UnityEngine;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(ConsumablesTableScreen))]
internal static class ConsumablesTableScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ConsumablesTableScreen.set_value_consumable_info))]
    private static void SetPermittedPatch(ConsumablesTableScreen __instance, GameObject widget_go)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;

        var widgetRow = __instance.rows.FirstOrDefault(row => row.rowType != TableRow.RowType.WorldDivider && row.ContainsWidget(widget_go));
        if (widgetRow == null)
            return;
        if (TableRow.RowType.Default != widgetRow.rowType)
            return;

        MultiplayerManager.Instance.NetClient.Send(new PermitConsumableByDefaultCommand(ConsumerManager.instance.DefaultForbiddenTagsList));
    }
}
