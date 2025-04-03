using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Commands.Tools.Args;
using MultiplayerMod.Core.Context;
using MultiplayerMod.Events;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands;

internal static class DragCommands
{
    internal static void DragToolCommand_Any(DragToolCommand toolCommand)
    {
        Debug.Log("DragToolCommand: " + toolCommand.DragToolType + " " + toolCommand.DragToolType.Name);
        Debug.Log("DragToolCommand: " + nameof(DigTool));
        switch (toolCommand.DragToolType.Name)
        {
            case nameof(DigTool):
                DragToolCommand_Dig(toolCommand.Args);
                break;
            case nameof(CancelTool):
                DragToolCommand_Cancel(toolCommand.Args);
                break;
            case nameof(DeconstructTool):
                DragToolCommand_Destruction(toolCommand.Args);
                break;
            case nameof(PrioritizeTool):
                DragToolCommand_Prioritize(toolCommand.Args);
                break;
            case nameof(DisinfectTool):
                DragToolCommand_Disinfect(toolCommand.Args);
                break;
            case nameof(ClearTool):
                DragToolCommand_Clear(toolCommand.Args);
                break;
            case nameof(AttackTool):
                DragToolCommand_Attack(toolCommand.Args);
                break;
            case nameof(MopTool):
                DragToolCommand_Mop(toolCommand.Args);
                break;
            case nameof(CaptureTool):
                DragToolCommand_Capture(toolCommand.Args);
                break;
            case nameof(HarvestTool):
                DragToolCommand_Harvest(toolCommand.Args);
                break;
            case nameof(EmptyPipeTool):
                DragToolCommand_EmptyPipe(toolCommand.Args);
                break;
            case nameof(DisconnectTool):
                DragToolCommand_Disconnect(toolCommand.Args);
                break;
            default:
                break;
        }

    }
    internal static void DragToolCommand_Cancel(DragCompleteEventArgs args)
    {
        var tool = new CancelTool();
        RunBasicCommandForTool(tool, args, () => { args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Destruction(DragCompleteEventArgs args)
    {
        var tool = new DeconstructTool();
        RunBasicCommandForTool(tool, args, () => { args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Dig(DragCompleteEventArgs args)
    {
        var tool = new DigTool();
        RunBasicCommandForTool(tool, args, () => { args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Disinfect(DragCompleteEventArgs args)
    {
        var tool = new DisinfectTool();
        RunBasicCommandForTool(tool, args, () => { args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_EmptyPipe(DragCompleteEventArgs args)
    {
        var tool = new EmptyPipeTool();
        RunBasicCommandForTool(tool, args, () => { args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Clear(DragCompleteEventArgs args)
    {
        var tool = new ClearTool();
        RunBasicCommandForTool(tool, args, () => { args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Attack(DragCompleteEventArgs args)
    {
        var tool = new AttackTool();
        RunBasicCommandForTool(tool, args, () => { tool.OnDragComplete(args.CursorDown, args.CursorUp); });
    }

    internal static void DragToolCommand_Disconnect(DragCompleteEventArgs args)
    {
        var tool = new DisconnectTool();
        RunBasicCommandForTool(tool, args, () => { tool.OnDragComplete(args.CursorDown, args.CursorUp); });
    }

    internal static void DragToolCommand_Capture(DragCompleteEventArgs args)
    {
        var tool = new CaptureTool();
        RunBasicCommandForTool(tool, args, () => { tool.OnDragComplete(args.CursorDown, args.CursorUp); });
    }

    internal static void DragToolCommand_Harvest(DragCompleteEventArgs args)
    {
        var tool = new HarvestTool();
        tool.downPos = args.CursorDown;

        tool.options = new Dictionary<string, ToolParameterMenu.ToggleState>
        {
            ["HARVEST_WHEN_READY"] = ToolParameterMenu.ToggleState.Off,
            ["DO_NOT_HARVEST"] = ToolParameterMenu.ToggleState.Off
        };
        args.Parameters?.ForEach(it => tool.options[it] = ToolParameterMenu.ToggleState.On);
        ContextRunner.Override(new PrioritySettingsContext(args.Priority), () => { tool.OnDragComplete(args.CursorDown, args.CursorUp); });
    }

    internal static void DragToolCommand_Mop(DragCompleteEventArgs args)
    {
        var tool = new MopTool();
        tool.downPos = args.CursorDown;
        tool.Placer = Assets.GetPrefab(new Tag("MopPlacer"));
        ContextRunner.Override(new PrioritySettingsContext(args.Priority), () => { tool.OnDragComplete(args.CursorDown, args.CursorUp); });
    }

    internal static void DragToolCommand_Prioritize(DragCompleteEventArgs args)
    {
        var tool = new PrioritizeTool();
        tool.downPos = args.CursorDown;

        if (tool is not FilteredDragTool filteredTool)
            return;

        filteredTool.currentFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>
        {
            [ToolParameterMenu.FILTERLAYERS.ALL] = ToolParameterMenu.ToggleState.Off
        };
        args.Parameters?.ForEach(it => filteredTool.currentFilterTargets[it] = ToolParameterMenu.ToggleState.On);
        ContextRunner.Override(new ContextArray(new PrioritySettingsContext(args.Priority), new DisablePriorityConfirmSound()), () => { tool.OnDragComplete(args.CursorDown, args.CursorUp); });
    }

    [NoAutoSubscribe]
    internal static void RunBasicCommandForTool<T>(T tool, DragCompleteEventArgs args, System.Action invokeAction) where T : DragTool, new()
    {
        tool.downPos = args.CursorDown;

        if (tool is not FilteredDragTool filteredTool)
        {
            ContextRunner.Override(new PrioritySettingsContext(args.Priority), invokeAction);
            return;
        }

        filteredTool.currentFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>
        {
            [ToolParameterMenu.FILTERLAYERS.ALL] = ToolParameterMenu.ToggleState.Off
        };
        args.Parameters?.ForEach(it => filteredTool.currentFilterTargets[it] = ToolParameterMenu.ToggleState.On);
        ContextRunner.Override(new PrioritySettingsContext(args.Priority), invokeAction);
    }
}
