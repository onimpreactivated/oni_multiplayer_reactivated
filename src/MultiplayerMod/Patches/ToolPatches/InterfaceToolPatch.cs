using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events;
using MultiplayerMod.Events.EventArgs;
using MultiplayerMod.Events.Others;
using UnityEngine;

namespace MultiplayerMod.Patches.ToolPatches;

[HarmonyPatch(typeof(InterfaceTool))]
internal static class InterfaceToolPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(InterfaceTool.OnMouseMove))]
    private static void OnMouseMove(Vector3 cursor_pos)
    {
        /*
        // Temporarly disable this.
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        var kScreens = KScreenManager.Instance.screenStack.Where(screen => screen.mouseOver).ToList();
        if (kScreens.Count == 0)
            return;
        var kScreen = kScreens.FirstOrDefault();
        if (kScreens.Count > 1)
        {
            kScreens.ForEach(Debug.Log);
        }
        EventManager.TriggerEvent<PlayerCursorPositionUpdatedEvent>(new(MultiplayerManager.Instance.MultiGame.Players.Current,
            new MouseMovedEventArgs(
                new Vector2(cursor_pos.x, cursor_pos.y),
                WorldToScreen(kScreen, cursor_pos),
                GetScreenName(kScreen),
                kScreen.GetType().ToString()
            )));
        */
    }

    private static string GetScreenName(KScreen screen) => screen switch
    {
        TableScreen tableScreen => tableScreen.title.ToLower(),
        RootMenu rootMenu => rootMenu.detailsScreen.displayName,
        _ => screen?.displayName
    };

    private static Vector2? WorldToScreen(KScreen screen, Vector2 cursorPos)
    {
        if (screen == null) return null;
        var screenRectTransform = screen.transform as RectTransform;
        if (screenRectTransform == null) return null;

        var screenPoint = Camera.main.WorldToScreenPoint(cursorPos);

        return new Vector2(
            (screenPoint.x - screenRectTransform.position.x) / screenRectTransform.rect.width,
            (screenPoint.y - screenRectTransform.position.y) / screenRectTransform.rect.height
        );
    }
}
