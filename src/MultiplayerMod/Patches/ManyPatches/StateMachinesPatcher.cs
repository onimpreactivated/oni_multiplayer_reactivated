using EIV_Common.Coroutines;
using HarmonyLib;
using MultiplayerMod.ChoreSync;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using System.Reflection;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch]
internal static class StateMachinesPatcher
{
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        return ChoreSyncList.GetStateMachineTypes().Select(x => x.GetMethod("InitializeStates"));
    }

    [HarmonyPostfix]
    internal static void PostStuff(object __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        Debug.Log("PostFix: " + __instance.GetType());
        Debug.Log("PostFix: " + __instance.GetType().DeclaringType);
        CoroutineWorkerCustom.CallDelayed(TimeSpan.FromMilliseconds(10), () => _StateMachineWork(__instance as StateMachine));

    }

    internal static void _StateMachineWork(StateMachine __instance)
    {
        switch (MultiplayerManager.Instance.MultiGame.Mode)
        {
            case Core.Player.PlayerRole.Server:
                ServerPostWork(__instance);
                break;
            case Core.Player.PlayerRole.Client:
                ClientPostWork(__instance);
                break;
            default:
                break;
        }
    }

    internal static void ServerPostWork(StateMachine __instance)
    {
        Type type = __instance.GetType();
        var state = ChoreSyncList.GetSync(type);
        if (state == default)
        {
            type = __instance.GetType().DeclaringType;
            state = ChoreSyncList.GetSync(type);
        }
        if (state == default)
        {
            Debug.Log($"State for Type {type} not yet been implemented.");
            return;
        }
        state.Server(__instance);
    }

    internal static void ClientPostWork(StateMachine __instance)
    {
        Type type = __instance.GetType();
        var state = ChoreSyncList.GetSync(type);
        if (state == default)
        {
            type = __instance.GetType().DeclaringType;
            state = ChoreSyncList.GetSync(type);
        }
        if (state == default)
        {
            Debug.Log($"State for Type {type} not yet been implemented.");
            return;
        }
        state.Client(__instance);
    }
}
