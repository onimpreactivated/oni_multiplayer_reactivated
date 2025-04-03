using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(JobsTableScreen))]
internal class JobsTableScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(JobsTableScreen.OnAdvancedModeToggleClicked))]
    internal static void OnAdvancedModeToggleClicked()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new SetPersonalPrioritiesAdvancedCommand(Game.Instance.advancedPersonalPriorities));
    }
}
