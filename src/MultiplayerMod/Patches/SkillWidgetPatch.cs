using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Core;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(SkillWidget))]
internal class SkillWidgetPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SkillWidget.OnPointerClick))]
    internal static void OnPointerClick(SkillWidget __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        __instance.skillsScreen.GetMinionIdentity(
            __instance.skillsScreen.CurrentlySelectedMinion,
            out var minionIdentity,
            out _
        );
        MultiplayerManager.Instance.NetClient.Send(new MasterSkillCommand(minionIdentity.gameObject.GetGOResolver(), __instance.skillID));
    }
}
