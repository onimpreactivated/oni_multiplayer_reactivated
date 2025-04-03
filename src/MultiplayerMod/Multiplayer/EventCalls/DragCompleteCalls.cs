using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Context;
using MultiplayerMod.Events.Arguments.Tools;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Patches;
using UnityEngine;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class DragCompleteCalls : BaseEventCall
{
    public override void Init()
    {
        ToolEvents.DragToolComplete += CopySettings;
        ToolEvents.DragToolComplete += DebugToolDragComplete;
        ToolEvents.DragToolComplete += DragCompletedEvent_Call;
    }

    internal static void CopySettings(DragToolCompleteArg @event)
    {
        if (@event.Sender is not CopySettingsTool)
            return;

        var lastSelection = GameStatePatch.LastSelectedObject;
        if (lastSelection == null)
            return;

        var cell = Grid.PosToCell(lastSelection.GetComponent<Transform>().GetPosition());

        var component = lastSelection.GetComponent<BuildingComplete>();
        if (component == null)
        {
            Debug.LogWarning($"Component 'BuildingComplete' not found in {lastSelection.GetProperName()} at cell #{cell}");
            return;
        }

        var layer = component.Def.ObjectLayer;

        MultiplayerManager.Instance.NetClient.Send(new CopySettingsCommand(new(@event.CommandArg, cell, layer)));
    }

    private static readonly HashSet<DebugTool.Type> modificationTypes =
    [
        DebugTool.Type.ReplaceSubstance,
        DebugTool.Type.FillReplaceSubstance,
        DebugTool.Type.Clear,
        DebugTool.Type.Deconstruct,
        DebugTool.Type.Destroy,
        DebugTool.Type.StoreSubstance
    ];

    internal static void DebugToolDragComplete(DragToolCompleteArg @event)
    {
        if (@event.Sender is not DebugTool tool)
            return;

        if (!modificationTypes.Contains(tool.type))
            return;

        var instance = DebugPaintElementScreen.Instance;
        var context = new DebugToolContext
        {
            Element = instance.paintElement.isOn ? instance.element : null,
            DiseaseType = instance.paintDisease.isOn ? instance.diseaseIdx : null,
            DiseaseCount = instance.paintDiseaseCount.isOn ? instance.diseaseCount : null,
            Temperature = instance.paintTemperature.isOn ? instance.temperature : null,
            Mass = instance.paintMass.isOn ? instance.mass : null,
            AffectCells = instance.affectCells.isOn,
            AffectBuildings = instance.affectBuildings.isOn,
            PreventFowReveal = instance.set_prevent_fow_reveal,
            AllowFowReveal = instance.set_allow_fow_reveal
        };

        MultiplayerManager.Instance.NetClient.Send(new ModifyCommand(new(@event.CommandArg, tool.type, context)));
    }

    internal static void DragCompletedEvent_Call(DragToolCompleteArg @event)
    {
        BaseCommandEvent command = null;
        switch (@event.Sender)
        {
            case DigTool:
                command = new DragToolCommand(@event.CommandArg, typeof(DigTool));
                break;
            case CancelTool:
                command = new DragToolCommand(@event.CommandArg, typeof(CancelTool));
                break;
            case DeconstructTool:
                command = new DragToolCommand(@event.CommandArg, typeof(DeconstructTool));
                break;
            case PrioritizeTool:
                command = new DragToolCommand(@event.CommandArg, typeof(PrioritizeTool));
                break;
            case DisinfectTool:
                command = new DragToolCommand(@event.CommandArg, typeof(DisinfectTool));
                break;
            case ClearTool:
                command = new DragToolCommand(@event.CommandArg, typeof(ClearTool));
                break;
            case AttackTool:
                command = new DragToolCommand(@event.CommandArg, typeof(AttackTool));
                break;
            case MopTool:
                command = new DragToolCommand(@event.CommandArg, typeof(MopTool));
                break;
            case CaptureTool:
                command = new DragToolCommand(@event.CommandArg, typeof(CaptureTool));
                break;
            case HarvestTool:
                command = new DragToolCommand(@event.CommandArg, typeof(HarvestTool));
                break;
            case EmptyPipeTool:
                command = new DragToolCommand(@event.CommandArg, typeof(EmptyPipeTool));
                break;
            case DisconnectTool:
                command = new DragToolCommand(@event.CommandArg, typeof(DisconnectTool));
                break;
            default:
                return;
        }

        MultiplayerManager.Instance.NetClient.Send(command);
    }
}
