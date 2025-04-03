using HarmonyLib;
using JetBrains.Annotations;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(MeterScreen))]
internal static class MeterScreenPatch
{
    [HarmonyPrefix, UsedImplicitly]
    [HarmonyPatch(nameof(MeterScreen.OnRedAlertClick))]
    private static void BeforeRedAlertClick()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;

        MultiplayerManager.Instance.NetClient.Send(new ChangeRedAlertStateCommand(!ClusterManager.Instance.activeWorld.AlertManager.IsRedAlertToggledOn()));
    }
}
