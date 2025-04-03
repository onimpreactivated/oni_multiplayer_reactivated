using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;
using MultiplayerMod.Patches.ScreenPatches;
using System.Reflection.Emit;
using UnityEngine;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(Telepad))]
internal class TelepadPatch
{
    internal static bool IsRequestedByCommand = false;
    internal static GameObject AcceptedGameObject;

    [HarmonyTranspiler]
    [HarmonyPatch(nameof(Telepad.OnAcceptDelivery))]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> newInstructions = instructions.ToList();

        // Getting the return index. (93 in U54-651155-SCR)
        int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ret);
        newInstructions.InsertRange(index,
        [
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(newInstructions[index]), // this
            new CodeInstruction(OpCodes.Ldarg_1), // delivery
            new CodeInstruction(OpCodes.Ldloc_1), // get local index 1 (gameObject)
            CodeInstruction.Call(typeof(TelepadPatch), nameof(OnAcceptDelivery))
            // TelepadPatch.OnAcceptDelivery(this, delivery, gameObject)
        ]);
        for (int z = 0; z < newInstructions.Count; z++)
            yield return newInstructions[z];
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Telepad.RejectAll))]
    private static void OnRejectAll(Telepad __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        ImmigrantScreenPatch.Deliverables = null;
        if (IsRequestedByCommand)
        {
            IsRequestedByCommand = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new RejectDeliveryCommand(__instance.GetComponentResolver()));
    }

    private static void OnAcceptDelivery(Telepad telepad, ITelepadDeliverable deliverable, GameObject gameObject)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        AcceptedGameObject = gameObject;
        ImmigrantScreenPatch.Deliverables = null;
        if (IsRequestedByCommand)
        {
            IsRequestedByCommand = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new AcceptDeliveryCommand(
                telepad.GetComponentResolver(),
                deliverable,
                gameObject.GetComponent<MultiplayerInstance>().Register(),
                gameObject.GetComponent<MinionIdentity>()?.GetMultiplayerInstance().Register()
            ));
    }
}
