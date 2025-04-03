using HarmonyLib;
using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

namespace MultiplayerMod.Patches.ToolPatches;

[HarmonyPatch(typeof(MoveToLocationTool))]
internal static class MoveToLocationToolPatch
{
    internal static bool IsCommandSent = false;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MoveToLocationTool.SetMoveToLocation))]
    internal static void SetMoveToLocationPostfix(MoveToLocationTool __instance, int target_cell)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (IsCommandSent)
        {
            IsCommandSent = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new MoveToLocationCommand(__instance.targetNavigator, __instance.targetMovable, target_cell));
    }
}
