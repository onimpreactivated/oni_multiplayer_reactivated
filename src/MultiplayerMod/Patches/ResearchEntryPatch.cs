using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(ResearchEntry))]
internal static class ResearchEntryPatch
{
    internal static bool IsExecutedByCommand = false;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ResearchEntry.OnResearchCanceled))]
    internal static void OnResearchCanceledPatch(ResearchEntry __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (IsExecutedByCommand)
        {
            IsExecutedByCommand = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new ResearchEntryCommand(__instance.targetTech.Id, true));
    }


    [HarmonyPostfix]
    [HarmonyPatch(nameof(ResearchEntry.OnResearchClicked))]
    internal static void OnResearchClickedPatch(ResearchEntry __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (IsExecutedByCommand)
        {
            IsExecutedByCommand = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new ResearchEntryCommand(__instance.targetTech.Id, false));
    }

}
