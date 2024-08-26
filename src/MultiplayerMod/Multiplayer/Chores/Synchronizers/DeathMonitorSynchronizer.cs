using HarmonyLib;
using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.ModRuntime.Context;
using MultiplayerMod.Multiplayer.Chores.Driver.Commands;
using MultiplayerMod.Multiplayer.StateMachines.Commands;
using MultiplayerMod.Network;

namespace MultiplayerMod.Multiplayer.Chores.Synchronizers;

[Dependency, UsedImplicitly]
[HarmonyPatch(typeof(DeathMonitor.Instance))]
public class DeathMonitorSynchronizer {

    private static IMultiplayerServer server = null!;
    private static MultiplayerGame multiplayer = null!;
    private static ExecutionLevelManager manager = null!;

    [HarmonyPrefix, UsedImplicitly]
    [HarmonyPatch(nameof(DeathMonitor.Instance.Kill))]
    private static bool KillPrefix(DeathMonitor.Instance __instance, Death death) {
        if (!manager.LevelIsActive(ExecutionLevel.Multiplayer))
            return true;

        if (multiplayer.Mode == MultiplayerMode.Client)
            return false;

        server.Send(new SetParameterValue(__instance, __instance.sm.death, death));
        server.Send(new ReleaseChoreDriver(__instance.controller.gameObject.GetComponent<ChoreDriver>()));
        return true;
    }

    public DeathMonitorSynchronizer(IMultiplayerServer server, MultiplayerGame multiplayer, ExecutionLevelManager manager) {
        DeathMonitorSynchronizer.server = server;
        DeathMonitorSynchronizer.multiplayer = multiplayer;
        DeathMonitorSynchronizer.manager = manager;
    }

}
