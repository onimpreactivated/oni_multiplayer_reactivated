using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using UnityEngine;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(UserMenu))]
internal static class UserMenuPatch
{
    internal static readonly Type[] skipButtonTypes =
    [
        // Camera specific actions
        typeof(Navigator),
        // Should be synced as a tool
        typeof(MoveToLocationMonitor.Instance),
        // Synced as a tool
        typeof(CopyBuildingSettings)
    ];

    [HarmonyPrefix]
    [HarmonyPatch(nameof(UserMenu.AddButton))]
    internal static void AddButton(GameObject go, ref KIconButtonMenu.ButtonInfo button)
    {
        var original = button.onClick;
        if (original == null)
            return;
        if (skipButtonTypes.Contains(original.Method.DeclaringType)) return;

        button.onClick = () => {
            original.Invoke();
            ExecutionManager.RunIfLevelIsActive(
                ExecutionLevel.Game,
                () => MultiplayerManager.Instance.NetClient.Send(new ClickUserMenuButtonCommand(go, original))
            );
        };
    }
}
