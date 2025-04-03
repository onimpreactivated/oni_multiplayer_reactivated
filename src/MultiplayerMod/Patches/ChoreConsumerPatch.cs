using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(ChoreConsumer))]
internal static class ChoreConsumerPatch
{
    internal static bool IsCommandSent = false;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ChoreConsumer.SetPersonalPriority))]
    internal static void SetPersonalPriority(ChoreConsumer __instance, ChoreGroup group, int value)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (IsCommandSent)
        {
            IsCommandSent = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new SetPersonalPriorityCommand(__instance.gameObject.GetGOResolver(), group.Id, value));
    }
}
