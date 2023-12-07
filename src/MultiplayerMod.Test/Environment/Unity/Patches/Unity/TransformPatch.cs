using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MultiplayerMod.Test.Environment.Unity.Patches.Unity;

[UsedImplicitly]
[HarmonyPatch(typeof(Transform))]
public class TransformPatch {

    [UsedImplicitly]
    [HarmonyTranspiler]
    [HarmonyPatch("get_position_Injected")]
    private static IEnumerable<CodeInstruction> Transform_get_position_Injected(
        IEnumerable<CodeInstruction> instructions
    ) {
        return new List<CodeInstruction> {
            new(OpCodes.Ldarg_0), // this
            new(OpCodes.Ldarg_1), // out result
            CodeInstruction.Call(typeof(UnityTestRuntime), nameof(UnityTestRuntime.GetPosition)),
            new(OpCodes.Ret)
        };
    }
}
