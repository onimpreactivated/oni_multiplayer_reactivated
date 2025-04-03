using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(RailGunSideScreen))]
internal static class RailGunSideScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(RailGunSideScreen.UpdateMaxCapacity))]
    internal static void UpdateMaxCapacity(RailGunSideScreen __instance, float newValue)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(
            new UpdateRailGunCapacityCommand(__instance.selectedGun.GetComponentResolver(), newValue)
            );
    }
}
