using System.Collections;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MultiplayerMod.Test.Environment.Unity.Patches.Unity;

[UsedImplicitly]
[HarmonyPatch(typeof(MonoBehaviour))]
public class MonoBehaviourPatch {

    [UsedImplicitly]
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MonoBehaviour.StartCoroutine), typeof(IEnumerator))]
    private static bool StartCoroutine(MonoBehaviour __instance, IEnumerator routine) {
        // Disabled for now, process if required.
        return false;
    }

}
