using EIV_Common.Coroutines;
using HarmonyLib;
using MultiplayerMod.ChoreSync;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Extensions;
using System.Reflection;

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
        if (__instance == null)
        {
            Debug.LogError("Instance is null!");
            return;
        }
        if (__instance is not StandardChoreBase standardChore)
        {
            Debug.Log("ChoreCTorPatcher: not StandardChoreBase: " + __instance.GetType());
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
        var serializable = chore.GetSMI().stateMachine.serializable;
        var id = chore.Register(persistent: serializable == StateMachine.SerializeType.Never);
        ChoresEvents.OnChoreCreated(chore, id, chore.GetType(), arguments);
    }
    private static void CancelChore(StandardChoreBase chore)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (chore == null)
            return;
        
        CoroutineWorkerCustom.StartCoroutine(_ChoreCancelWait(chore), CoroutineType.Custom, "CancelWait");
    }

    internal static IEnumerator<double> _ChoreCancelWait(StandardChoreBase chore)
    {
        yield return 0;
        Debug.Log("Cancel Chore: " + chore.GetType());
        string reason = $"Chore instantiation of type \"{chore.GetType()}\" is disabled";
        chore.Cancel(reason);
        Debug.Log("Cancel Success!");
        yield break;

    }
}
