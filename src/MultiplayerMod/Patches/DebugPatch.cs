using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

namespace MultiplayerMod.Patches;

[HarmonyPatch]
internal static class DebugPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SpeedControlScreen), nameof(SpeedControlScreen.DebugStepFrame))]
    private static void DebugStepFramePrefix()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new DebugGameFrameStepCommand());
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(global::Game), nameof(global::Game.ForceSimStep))]
    private static void ForceSimStepPrefix()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new DebugSimulationStepCommand());
    }
}
