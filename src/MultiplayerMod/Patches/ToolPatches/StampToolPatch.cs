using HarmonyLib;
using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;
using System.Reflection.Emit;
using UnityEngine;

namespace MultiplayerMod.Patches.ToolPatches;

[HarmonyPatch(typeof(StampTool))]
[HarmonyPriority(Priority.High)]
internal static class StampToolPatch
{
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(StampTool.Stamp))]
    internal static IEnumerable<CodeInstruction> StampTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        using var source = instructions.GetEnumerator();
        var result = new List<CodeInstruction>();

        var templateLoaderStampMethod = AccessTools.Method(typeof(TemplateLoader), nameof(TemplateLoader.Stamp));
        result.AddConditional(source, it => it.Calls(templateLoaderStampMethod));

        // Add StampToolEvents.OnStamp(this, pos) after TemplateLoader.Stamp
        result.Add(new CodeInstruction(OpCodes.Ldarg_0)); // this
        result.Add(new CodeInstruction(OpCodes.Ldarg_1)); // pos
        result.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(StampToolPatch), nameof(OnStamp))));

        result.AddConditional(source, _ => false);

        return result;
    }

    private static void OnStamp(StampTool instance, Vector2 location)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new StampCommand(instance.stampTemplate, location));
    }
}
