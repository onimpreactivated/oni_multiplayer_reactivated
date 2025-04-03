using MultiplayerMod.Commands;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Events.Others;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class UICalls
{
    internal static readonly CommandRateThrottle Throttle10Hz = new(rate: 10);

    internal static void OnConnectionLost(ConnectionLostEvent _)
    {
        var screen = (InfoDialogScreen) GameScreenManager.Instance.StartScreen(
            ScreenPrefabs.Instance.InfoDialogScreen.gameObject,
            GameScreenManager.Instance.ssOverlayCanvas.gameObject
        );
        screen.SetHeader("Multiplayer");
        screen.AddPlainText("Connection has been lost. Further play can not be synced");
        screen.AddOption(
            "OK",
            _ => PauseScreen.Instance.OnQuitConfirm(false)
        );
    }

    internal static void PlayerCursorPositionUpdatedEvent_Event(PlayerCursorPositionUpdatedEvent @event)
    {
        if (@event.Player != MultiplayerManager.Instance.MultiGame.Players.Current)
            return;
        Throttle10Hz.Run<UpdatePlayerCursorPositionCommand>(() =>
        {
            MultiplayerManager.Instance.NetClient.Send(new UpdatePlayerCursorPositionCommand(@event.Player.Id, @event.EventArgs));
        });
    }
}
