using HarmonyLib;

namespace MultiplayerMod.Core.Context;

[HarmonyPatch(typeof(PriorityScreen))]
internal class DisablePriorityConfirmSound : IContext
{
    private static bool enabled = true;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PriorityScreen.PlayPriorityConfirmSound))]
    internal static bool PlayPriorityConfirmSoundPrefix() => enabled;

    public void Apply() => enabled = false;

    public void Restore() => enabled = true;
}
