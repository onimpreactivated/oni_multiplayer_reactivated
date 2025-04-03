using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Extensions;
using MultiplayerMod.Patches;

namespace MultiplayerMod.Commands;

internal class PriorityCommands
{
    internal static void ChangePriorityCommand_Event(ChangePriorityCommand command)
    {
        command.Target.Resolve().SetMasterPriority(command.Priority);
    }

    internal static void SetDefaultPriorityCommand_Event(SetDefaultPriorityCommand command)
    {
        ImmigrationPatch.IsCommandSent = true;
        global::Immigration.Instance.SetPersonalPriority(command.ChoreGroup, command.Value);
        var screen = ManagementMenu.Instance.jobsScreen;
        foreach (var row in screen.rows)
        {
            var minion = row.GetIdentity();
            row.widgets
                .Where(entry => entry.Key is PrioritizationGroupTableColumn)
                .Select(entry => entry.Value)
                .ForEach(widget => screen.LoadValue(minion, widget));
        }
    }

    internal static void SetPersonalPriorityCommand_Event(SetPersonalPriorityCommand command)
    {
        ChoreConsumerPatch.IsCommandSent = true;
        var choreConsumer = command.ChoreConsumerReference.Resolve().GetComponent<ChoreConsumer>();
        if (choreConsumer == null) return;
        choreConsumer.SetPersonalPriority(command.ChoreGroup, command.Value);
        var screen = ManagementMenu.Instance.jobsScreen;
        foreach (var row in screen.rows)
        {
            var minion = row.GetIdentity();
            row.widgets
                .Where(entry => entry.Key is PrioritizationGroupTableColumn)
                .Select(entry => entry.Value)
                .ForEach(widget => screen.LoadValue(minion, widget));
        }
    }

    internal static void SetPersonalPrioritiesAdvancedCommand_Event(SetPersonalPrioritiesAdvancedCommand command)
    {
        global::Game.Instance.advancedPersonalPriorities = command.IsAdvanced;
        ManagementMenu.Instance.jobsScreen.toggleAdvancedModeButton.fgImage.gameObject.SetActive(command.IsAdvanced);
    }
}
