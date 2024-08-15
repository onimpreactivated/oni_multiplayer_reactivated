using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MultiplayerMod.Test.Environment.Unity.Patches.Unity;

[UsedImplicitly]
[HarmonyPatch(typeof(SystemInfo))]
public class SystemInfoPatch {

    [UsedImplicitly]
    [HarmonyTranspiler]
    [HarmonyPatch("get_processorCount")]
    private static IEnumerable<CodeInstruction> SystemInfo_get_processorCount(
        IEnumerable<CodeInstruction> instructions
    ) {
        return new List<CodeInstruction> {
            new(OpCodes.Ldc_I4_1), // 1
            new(OpCodes.Ret)
        };
    }

    [UsedImplicitly]
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(SystemInfo.operatingSystem), MethodType.Getter)]
    private static IEnumerable<CodeInstruction> SystemInfo_get_OperatingSystem(
        IEnumerable<CodeInstruction> instructions
    ) {
        return new List<CodeInstruction> {
            new(OpCodes.Ldstr, "Windows 11 (10.0.22621) 64bit"),
            new(OpCodes.Ret)
        };
    }

}
