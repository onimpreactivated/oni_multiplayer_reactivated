using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches;

internal class SetHatEventPatch
{
    [HarmonyPatch(typeof(SkillsScreen))]
    internal static class SkillsScreenEvents
    {

        [HarmonyPostfix]
        [HarmonyPatch(nameof(SkillsScreen.OnHatDropEntryClick))]
        internal static void OnHatDropEntryClick(SkillsScreen __instance, IListableOption skill)
        {
            if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
                return;
            __instance.GetMinionIdentity(__instance.currentlySelectedMinion, out var minionIdentity, out _);
            MultiplayerManager.Instance.NetClient.Send(new SetHatCommand(minionIdentity.gameObject.GetGOResolver(), (skill as HatListable).hat));
        }

    }

    [HarmonyPatch(typeof(SkillMinionWidget))]
    internal static class SkillMinionWidgetEvents
    {

        [HarmonyPostfix]
        [HarmonyPatch(nameof(SkillMinionWidget.OnHatDropEntryClick))]
        internal static void OnHatDropEntryClick(SkillMinionWidget __instance, IListableOption hatOption)
        {
            if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
                return;
            __instance.skillsScreen.GetMinionIdentity(__instance.assignableIdentity, out var minionIdentity, out _);
            MultiplayerManager.Instance.NetClient.Send(new SetHatCommand(minionIdentity.gameObject.GetGOResolver(), (hatOption as HatListable).hat));
        }

    }
}
