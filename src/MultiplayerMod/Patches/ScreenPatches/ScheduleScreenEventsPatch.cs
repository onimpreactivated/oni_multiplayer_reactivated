using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;

namespace MultiplayerMod.Patches.ScreenPatches;

internal class ScheduleScreenEventsPatch
{
    internal static bool IsCommandSent = false;

    internal static void TriggerChange()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (IsCommandSent)
        {
            IsCommandSent = false;
            return;
        }
        MultiplayerManager.Instance.NetClient.Send(new ChangeSchedulesListCommand(ScheduleManager.Instance.schedules));
    }

    [HarmonyPatch(typeof(ScheduleScreen))]
    internal static class ScheduleScreenPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch(nameof(ScheduleScreen.OnAddScheduleClick))]
        internal static void OnAddScheduleClick()
        {
            TriggerChange();
        }
    }

    [HarmonyPatch(typeof(ScheduleScreenEntry))]
    internal static class ScheduleScreenEntryPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(ScheduleScreenEntry.OnDeleteClicked))]
        internal static void OnDeleteClicked()
        {
            TriggerChange();
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(ScheduleScreenEntry.OnScheduleChanged))]
        internal static void OnScheduleChanged()
        {
            TriggerChange();
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(ScheduleScreenEntry.OnNameChanged))]
        internal static void OnNameChanged()
        {
            TriggerChange();
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(ScheduleScreenEntry.OnAlarmClicked))]
        internal static void OnAlarmClicked()
        {
            TriggerChange();
        }

    }
}
