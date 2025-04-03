using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(UserMenuScreen))]
internal static class UserMenuScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(UserMenuScreen.OnPriorityClicked))]
    internal static void OnPriorityClicked(UserMenuScreen __instance, PrioritySetting priority)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (__instance.selected == null)
            return;

        var component = __instance.selected.GetComponent<Prioritizable>();
        if (component == null)
            return;

        MultiplayerManager.Instance.NetClient.Send(new ChangePriorityCommand(component.GetComponentResolver(), priority));
    }
}
