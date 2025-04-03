using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Commands.Tools.Args;
using MultiplayerMod.Core.Context;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.Patches.ToolPatches;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Mono.Cecil.Mixin;
using static STRINGS.ELEMENTS;

namespace MultiplayerMod.Commands;

internal class BuildCommands
{
    internal static void BuildUtilityCommand_UtilityBuildTool_Event(BuildUtilityCommand command)
    {
        var definition = Assets.GetBuildingDef(command.Args.PrefabId);
        BaseUtilityBuildTool tool = null;
        if (command.SenderType.Name == nameof(WireBuildTool))
        {
            tool = new WireBuildTool()
            {
                def = definition,
                conduitMgr = definition.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>().GetNetworkManager(),
                selectedElements = command.Args.Materials,
                path = command.Args.Path
            };
        }
        else
        {
            tool = new BaseUtilityBuildTool()
            {
                def = definition,
                conduitMgr = definition.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>().GetNetworkManager(),
                selectedElements = command.Args.Materials,
                path = command.Args.Path
            };
        }

        BaseUtilityBuildToolPatch.IsCommandSent = true;
        ContextRunner.Override(new PrioritySettingsContext(command.Args.Priority), tool.BuildPath);
    }
    internal static void BuildCommand_Event(BuildCommand command)
    {
        var definition = Assets.GetBuildingDef(command.Args.PrefabId);
        var cbcPosition = Grid.CellToPosCBC(command.Args.Cell, Grid.SceneLayer.Building);
        ContextRunner.Override(new DisableBuildingValidation(), () => Execute(definition, cbcPosition, command.Args));
    }


    private static void Execute(BuildingDef definition, Vector3 cbcPosition, BuildEventArgs args)
    {
        var building = args.Upgrade ? DoUpgrade(definition, cbcPosition, args) : DoBuild(definition, cbcPosition, args);
        if (building != null)
            ConfigureBuilding(building, args);
    }

    private static GameObject DoBuild(BuildingDef definition, Vector3 cbcPosition, BuildEventArgs args)
    {
        return args.InstantBuild
            ? DoInstantBuild(definition, args)
            : definition.TryPlace(null, cbcPosition, args.Orientation, args.Materials, args.FacadeId);
    }

    private static GameObject DoUpgrade(BuildingDef definition, Vector3 cbcPosition, BuildEventArgs args)
    {
        if (args.InstantBuild)
        {
            var candidate = definition.GetReplacementCandidate(args.Cell);
            return InstantUpgrade(definition, candidate, args);
        }
        var item = definition.TryReplaceTile(null, cbcPosition, args.Orientation, args.Materials, args.FacadeId);
        Grid.Objects[args.Cell, (int) definition.ReplacementLayer] = item;
        return item;
    }

    public static GameObject InstantUpgrade(BuildingDef definition, GameObject candidate, BuildEventArgs args)
    {
        if (candidate.GetComponent<SimCellOccupier>() == null)
        {
            UnityObjectManager.Destroy(candidate);
            return DoInstantBuild(definition, args);
        }
        candidate.GetComponent<SimCellOccupier>().DestroySelf(
            () => {
                UnityObjectManager.Destroy(candidate);
                ConfigureBuilding(DoInstantBuild(definition, args), args);
            }
        );
        return null;
    }

    private static void ConfigureBuilding(GameObject builtItem, BuildEventArgs args)
    {
        if (!args.InstantBuild)
        {
            var prioritizable = builtItem.GetComponent<Prioritizable>();
            prioritizable?.SetMasterPriority(args.Priority);
        }
        var rotatable = builtItem.GetComponent<Rotatable>();
        rotatable?.SetOrientation(args.Orientation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static GameObject DoInstantBuild(BuildingDef definition, BuildEventArgs args) => definition.Build(
        args.Cell,
        args.Orientation,
        null,
        args.Materials,
        293.15f,
        args.FacadeId,
        false,
        GameClock.Instance.GetTime()
    );


    internal static void CopySettingsCommand_Event(CopySettingsCommand command)
    {
        ContextRunner.Override(new DisablePopUpEffects(), () => CopySettingsExecute(command.Args));
    }

    private static void CopySettingsExecute(CopySettingsEventArgs args)
    {
        var tool = new CopySettingsTool();
        var source = Grid.Objects[args.SourceCell, (int) args.SourceLayer];
        if (source == null)
        {
            Debug.LogWarning($"Unable to locate source at cell #{args.SourceCell} on {args.SourceLayer} layer");
            return;
        }
        tool.SetSourceObject(source);
        args.DragEvent.Cells.ForEach(it => tool.OnDragTool(it, 0));
    }

    internal static void ModifyCommand_Event(ModifyCommand command)
    {
        var tool = new DebugTool
        {
            type = command.Args.Type
        };
        ContextRunner.Override(
            command.Args.ToolContext,
            () => { command.Args.DragEventArgs.Cells.ForEach(it => tool.OnDragTool(it, 0)); }
        );
    }

    internal static void MoveToLocationCommand_Event(MoveToLocationCommand command)
    {
        var navigator = command.NavigatorReference?.Resolve();
        var movable = command.MovableReference?.Resolve();

        if (navigator != null)
            navigator.GetSMI<MoveToLocationMonitor.Instance>()?.MoveToLocation(command.Cell);
        else if (movable != null)
            movable.MoveToLocation(command.Cell);
    }

    internal static void StampCommand_Event(StampCommand command)
    {
        var tool = new StampTool
        {
            stampTemplate = command.Template,
            ready = true,
            selectAffected = false,
            deactivateOnStamp = false
        };
        ContextRunner.Override(new StampCompletionOverride(), () => tool.Stamp(command.Location));

    }
}
