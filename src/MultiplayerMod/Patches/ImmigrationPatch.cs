using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

[HarmonyPatch(typeof(Immigration))]
internal static class ImmigrationPatch
{
    internal static bool IsCommandSent = false;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Immigration.SetPersonalPriority))]
    internal static void SetPersonalPriority(ChoreGroup group, int value)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (IsCommandSent)
        {
            IsCommandSent = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new SetDefaultPriorityCommand(group.Id, value));
    }

}
