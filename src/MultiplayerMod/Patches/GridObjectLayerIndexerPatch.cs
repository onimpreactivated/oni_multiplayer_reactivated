using HarmonyLib;
using MultiplayerMod.Core.Behaviour;
using UnityEngine;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(Grid.ObjectLayerIndexer))]
internal static class GridObjectLayerIndexerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("set_Item")]
    internal static void SetItemPostfix(int layer, GameObject value)
    {
        if (value == null)
            return;

        var extension = value.GetComponent<GridObject>();
        if (extension == null)
            return;

        extension.GridLayer = layer;
    }
}
