using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches;

//[HarmonyPatch(typeof(ConsumableConsumer))]
internal static class ConsumableConsumerPatch
{
    internal static bool IsCommandSent = false;
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ConsumableConsumer.SetPermitted))]
    internal static void SetPermittedPatch(ConsumableConsumer __instance, string consumable_id, bool is_allowed)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (IsCommandSent)
        {
            IsCommandSent = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new PermitConsumableToMinionCommand(__instance.gameObject.GetGOResolver(), consumable_id, is_allowed));
    }
}
