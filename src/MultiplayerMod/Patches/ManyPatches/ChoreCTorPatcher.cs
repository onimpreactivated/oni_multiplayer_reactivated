using EIV_Common.Coroutines;
using HarmonyLib;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Chores;
using MultiplayerMod.Events;
using MultiplayerMod.Extensions;
using System.Reflection;
using MultiplayerMod.ChoreSync;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch]
internal static class ChoreCTorPatcher
{
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        return ChoreSyncList.GetSyncTypes().Select(x => x.GetConstructors()[0]);
    }

    [HarmonyPostfix]
    internal static void Chore_Ctor_Patch(Chore __instance, object[] __args)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (!MultiplayerManager.IsMultiplayer())
            return;
        if (__instance is not StandardChoreBase standardChore)
        {
            Debug.Log("ChoreCTorPatcher: " + __instance.GetType());
            return;
        }
        switch (MultiplayerManager.Instance.MultiGame.Mode)
        {
            case Core.Player.PlayerRole.Server:
                OnChoreCreated(standardChore, __args);
                break;
            case Core.Player.PlayerRole.Client:
                CancelChore(standardChore);
                break;
        }
    }

    private static void OnChoreCreated(StandardChoreBase chore, object[] arguments)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        CoroutineWorkerCustom.StartCoroutine(_ChoreCreateWait(chore, arguments), CoroutineType.Custom, "CreateWait");
    }
    private static void CancelChore(StandardChoreBase chore)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (chore == null)
            return;
        CoroutineWorkerCustom.StartCoroutine(_ChoreCancelWait(chore), CoroutineType.Custom, "CancelWait");
    }

    internal static IEnumerator<double> _ChoreCreateWait(StandardChoreBase chore, object[] arguments)
    {
        yield return TimeSpan.FromMilliseconds(10).TotalSeconds;
        StateMachine.Instance smi = null;
        yield return CoroutineWorkerCustom.WaitUntilTrue(() =>
        {
            Debug.Log($"Create Wait Chore! {chore.GetType()}");
            smi = chore.GetSMI();
            return smi != null;
        });
        yield return 0;
        StateMachine statemachine = smi.stateMachine;
        yield return CoroutineWorkerCustom.WaitUntilTrue(() =>
        {
            statemachine = smi.stateMachine;
            return statemachine != null;
        });
        yield return 0;
        var seri = statemachine.serializable;
        var id = chore.Register(persistent: seri == StateMachine.SerializeType.Never);
        Debug.Log($"Register Success! {id}, {chore.GetType()}");
        Debug.Log($"Register Success! {chore.IsValid_Ext()} | {chore.MultiplayerId()}");
        EventManager.TriggerEvent(new ChoreCreatedEvent(chore, id, chore.GetType(), arguments));
        yield break;
    }

    internal static IEnumerator<double> _ChoreCancelWait(StandardChoreBase chore)
    {
        yield return TimeSpan.FromMilliseconds(10).TotalSeconds;
        yield return CoroutineWorkerCustom.WaitUntilTrue(() =>
        {
            Debug.Log($"Cancel Wait Chore! {chore.GetType()}");
            var smi = chore.GetSMI();
            return smi != null;
        });
        yield return 0;
        Debug.Log("Cancel Chore: " + chore.GetType());
        string reason = $"Chore instantiation of type \"{chore.GetType()}\" is disabled";
        chore.Cancel(reason);
        Debug.Log("Cancel Success!");
        yield break;

    }
}
