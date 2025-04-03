using HarmonyLib;
using MultiplayerMod.Commands.Tools.Args;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Core;
using MultiplayerMod.Commands.Tools;

namespace MultiplayerMod.Patches.ToolPatches;

[HarmonyPatch(typeof(BaseUtilityBuildTool))]
internal static class BaseUtilityBuildToolPatch
{
    internal static bool IsCommandSent = false;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(BaseUtilityBuildTool.BuildPath))]
    internal static void BuildPathPrefix(BaseUtilityBuildTool __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (IsCommandSent)
        {
            IsCommandSent = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new BuildUtilityCommand(new UtilityBuildEventArgs(
            __instance.def.PrefabID,
            [.. __instance.selectedElements],
            __instance.path,
            GameStatePatch.BuildToolPriority
        ), __instance.GetType()));
    }
}
