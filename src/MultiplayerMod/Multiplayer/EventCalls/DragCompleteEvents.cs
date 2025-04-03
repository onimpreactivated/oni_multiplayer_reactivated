using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Context;
using MultiplayerMod.Events.Others;
using MultiplayerMod.Patches;
using UnityEngine;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class DragCompleteEvents
{

    internal static void CopySettings(DragCompletedEvent @event)
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

        MultiplayerManager.Instance.NetClient.Send(new CopySettingsCommand(new(@event.Args, cell, layer)));
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

    internal static void DebugToolDragComplete(DragCompletedEvent @event)
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

        MultiplayerManager.Instance.NetClient.Send(new ModifyCommand(new(@event.Args, tool.type, context)));
    }

    internal static void DragCompletedEvent_Call(DragCompletedEvent @event)
    {
        BaseCommandEvent command = null;
        switch (@event.Sender)
        {
            case DigTool:
                command = new DragToolCommand(@event.Args, typeof(DigTool));
                break;
            case CancelTool:
                command = new DragToolCommand(@event.Args, typeof(CancelTool));
                break;
            case DeconstructTool:
                command = new DragToolCommand(@event.Args, typeof(DeconstructTool));
                break;
            case PrioritizeTool:
                command = new DragToolCommand(@event.Args, typeof(PrioritizeTool));
                break;
            case DisinfectTool:
                command = new DragToolCommand(@event.Args, typeof(DisinfectTool));
                break;
            case ClearTool:
                command = new DragToolCommand(@event.Args, typeof(ClearTool));
                break;
            case AttackTool:
                command = new DragToolCommand(@event.Args, typeof(AttackTool));
                break;
            case MopTool:
                command = new DragToolCommand(@event.Args, typeof(MopTool));
                break;
            case CaptureTool:
                command = new DragToolCommand(@event.Args, typeof(CaptureTool));
                break;
            case HarvestTool:
                command = new DragToolCommand(@event.Args, typeof(HarvestTool));
                break;
            case EmptyPipeTool:
                command = new DragToolCommand(@event.Args, typeof(EmptyPipeTool));
                break;
            case DisconnectTool:
                command = new DragToolCommand(@event.Args, typeof(DisconnectTool));
                break;
            default:
                return;
        }

        MultiplayerManager.Instance.NetClient.Send(command);
    }
}
