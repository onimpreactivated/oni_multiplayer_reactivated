using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Others;
using MultiplayerMod.Patches.ScreenPatches;
using System.Reflection;

namespace MultiplayerMod.Commands;

internal class UICommands
{
    public static void UpdatePlayerCursorPositionCommand_Event(UpdatePlayerCursorPositionCommand command)
    {
        var player = MultiplayerManager.Instance.MultiGame.Players[command.PlayerId];
        EventManager.TriggerEvent(new PlayerCursorPositionUpdatedEvent(player, command.EventArgs));

    }

    public static void InitializeImmigrationCommand_Event(InitializeImmigrationCommand command)
    {
        ImmigrantScreenPatch.Deliverables = command.Deliverables;
    }

    public static void ClickUserMenuButtonCommand_Event(ClickUserMenuButtonCommand command)
    {
        try
        {
            var methodInfo = command.ActionDeclaringType.GetMethod(
                command.ActionName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly,
                null,
                [],
                []
            );
            var target = command.Resolver.Resolve();
            methodInfo?.Invoke(target.GetComponent(command.ActionDeclaringType), []);
            target.Trigger((int) GameHashes.RefreshUserMenu);
            target.Trigger((int) GameHashes.UIRefresh);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
}
