using HarmonyLib;
using System.Reflection;

namespace MultiplayerMod.Core.Context;

[HarmonyPatch]
internal class DisablePopUpEffects : IContext
{
    internal static bool enabled = true;

    internal static IEnumerable<MethodBase> TargetMethods() =>
        typeof(PopFXManager).GetMethods()
            .Where(it => it.Name.StartsWith("SpawnFX"));

    [HarmonyPrefix]
    internal static bool SpawnFxPrefix() => enabled;

    public void Apply()
    {
        enabled = false;
    }

    public void Restore()
    {
        enabled = true;
    }
}
