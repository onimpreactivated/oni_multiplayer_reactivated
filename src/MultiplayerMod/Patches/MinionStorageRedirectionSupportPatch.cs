using HarmonyLib;
using MultiplayerMod.Core.Execution;
using UnityEngine;
using MultiplayerMod.Core.Behaviour;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(MinionStorage))]
internal static class MinionStorageRedirectionSupportPatch
{

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MinionStorage.RedirectInstanceTracker))]
    internal static void RedirectInstanceTracker(GameObject src_minion, GameObject dest_minion)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        src_minion.GetComponent<MultiplayerInstance>().Redirect(dest_minion.GetComponent<MultiplayerInstance>());
    }
}
