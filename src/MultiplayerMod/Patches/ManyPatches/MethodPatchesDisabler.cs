using HarmonyLib;
using MultiplayerMod.Core.Execution;
using System.Reflection;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch]
internal static class MethodPatchesDisabler
{
    internal static readonly PatchTargetResolver targets = new PatchTargetResolver.Builder()
        .AddMethods(typeof(MinionIdentity), nameof(MinionIdentity.OnSpawn))
        .AddMethods(typeof(MinionStartingStats), nameof(MinionStartingStats.Deliver))
        .AddMethods(typeof(SaveLoader), nameof(SaveLoader.InitialSave))
        .AddMethods(typeof(MinionStorage), nameof(MinionStorage.CopyMinion))
        .Build();

    [HarmonyTargetMethods]
    internal static IEnumerable<MethodBase> TargetMethods() => targets.Resolve();

    [HarmonyPrefix]
    internal static void BeforeMethod()
    {
        if (ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            ExecutionManager.EnterOverrideSection(ExecutionLevel.Component);
    }

    [HarmonyPostfix]
    internal static void AfterMethod()
    {
        if (ExecutionManager.LevelIsActive(ExecutionLevel.Component))
            ExecutionManager.LeaveOverrideSection();
    }
}
