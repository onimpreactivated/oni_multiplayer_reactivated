using HarmonyLib;
using UnityEngine;

namespace MultiplayerMod.Patches;

[HarmonyPatch]
internal static class GameStatePatch
{
    public static GameObject LastSelectedObject { get; private set; }
    public static PrioritySetting BuildToolPriority => GetBuildToolPriority();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(global::Game), nameof(global::Game.OnPrefabInit))]
    internal static void OnGamePrefabInit(global::Game __instance)
    {
        __instance.Subscribe(
            (int) GameHashes.SelectObject,
            o => {
                if (o != null)
                    LastSelectedObject = (GameObject) o;
            }
        );
    }

    internal static PrioritySetting GetBuildToolPriority()
    {
        var priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 0);

        // Reference: BaseUtilityBuildTool.BuildPath, BuildTool.PostProcessBuild
        if (BuildMenu.Instance != null)
            priority = BuildMenu.Instance.GetBuildingPriority();
        if (PlanScreen.Instance != null)
            priority = PlanScreen.Instance.GetBuildingPriority();

        return priority;
    }
}
