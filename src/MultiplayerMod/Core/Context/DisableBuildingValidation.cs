using HarmonyLib;
using System.Reflection;

namespace MultiplayerMod.Core.Context;

[HarmonyPatch]
internal class DisableBuildingValidation : IContext
{
    internal static bool validationEnabled = true;

    internal static IEnumerable<MethodBase> TargetMethods() =>
        typeof(BuildingDef).GetMethods()
            .Where(it => it.Name.StartsWith("IsValidPlaceLocation"));

    [HarmonyPrefix]
    internal static bool IsValidPlaceLocationPrefix(ref bool __result)
    {
        if (validationEnabled)
            return true;

        __result = true;
        return false;
    }

    public void Apply()
    {
        validationEnabled = false;
    }

    public void Restore()
    {
        validationEnabled = true;
    }
}
