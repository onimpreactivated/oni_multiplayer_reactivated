using HarmonyLib;
using MultiplayerMod.Commands.Tools.Args;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Others;
using System.Reflection;
using UnityEngine;

namespace MultiplayerMod.Patches.ToolPatches;

internal static class DragToolEvents
{
    internal static DragTool lastTool;
    internal static readonly List<int> selection = [];

    internal static readonly Type[] onDragToolClasses =
    [
        typeof(DragTool),
        typeof(DigTool),
        typeof(CancelTool),
        typeof(DeconstructTool),
        typeof(PrioritizeTool),
        typeof(DisinfectTool),
        typeof(ClearTool),
        typeof(MopTool),
        typeof(HarvestTool),
        typeof(EmptyPipeTool),
        typeof(DebugTool),
        typeof(CopySettingsTool)
    ];

    internal static readonly Type[] onDragCompleteClasses =
    [
        typeof(DragTool),
        typeof(CancelTool),
        typeof(AttackTool),
        typeof(CaptureTool),
        typeof(DisconnectTool)
    ];

    [HarmonyPatch]
    internal class OnDragToolPatch
    {
        internal static IEnumerable<MethodBase> TargetMethods()
            => Assembly.GetAssembly(typeof(DragTool))
                .GetTypes()
                .Where(type => onDragToolClasses.Contains(type))
                .Select(
                    type => type.GetMethod(
                        nameof(DragTool.OnDragTool),
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
                    )
                );

        [HarmonyPostfix]
        internal static void DragToolOnDragToolPostfix(DragTool __instance, int cell) => AddDragCell(__instance, cell);

        internal static void AddDragCell(DragTool __instance, int cell)
        {
            if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
                return;
            AssertSameInstance(__instance);
            selection.Add(cell);
            lastTool = __instance;
        }

    }

    [HarmonyPatch]
    internal class OnDragCompletePatch
    {

        internal static IEnumerable<MethodBase> TargetMethods()
            => Assembly.GetAssembly(typeof(DragTool))
                .GetTypes()
                .Where(type => onDragCompleteClasses.Contains(type))
                .Select(
                    type => type.GetMethod(
                        nameof(DragTool.OnDragComplete),
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
                    )
                );

        [HarmonyPostfix]
        internal static void DragToolOnDragCompletePostfix(DragTool __instance, Vector3 __0, Vector3 __1) =>
            CompleteDrag(__instance, __0, __1);

        internal static void CompleteDrag(DragTool instance, Vector3 cursorDown, Vector3 cursorUp)
        {
            if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
                return;
            AssertSameInstance(instance);

            var args = new DragCompleteEventArgs(
                selection,
                cursorDown,
                cursorUp,
                ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority(),
                instance switch
                {
                    FilteredDragTool filtered => GetActiveParameters(filtered.currentFilterTargets),
                    HarvestTool harvest => GetActiveParameters(harvest.options),
                    _ => null
                }
            );

            EventManager.TriggerEvent(new DragCompletedEvent(instance, args));

            selection.Clear();
            lastTool = null;
        }

        internal static string[] GetActiveParameters(Dictionary<string, ToolParameterMenu.ToggleState> parameters)
        {
            return parameters.Where(it => it.Value == ToolParameterMenu.ToggleState.On).Select(it => it.Key).ToArray();
        }
    }

    internal static void AssertSameInstance(DragTool instance)
    {
        if (lastTool != null && lastTool != instance)
            throw new Exception("Concurrent drag events detected");
    }
}
