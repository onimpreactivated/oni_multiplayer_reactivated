using MultiplayerMod.Commands;
using MultiplayerMod.Events.Handlers;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class UICalls : BaseEventCall
{
    internal static readonly CommandRateThrottle Throttle10Hz = new(rate: 10);
    public override void Init()
    {
        MultiplayerEvents.ConnectionLost += OnConnectionLost;
    }

    internal static void OnConnectionLost()
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
    /*
    internal static void PlayerCursorPositionUpdatedEvent_Event(PlayerCursorPositionUpdatedEvent @event)
    {
        if (@event.Player != MultiplayerManager.Instance.MultiGame.Players.Current)
            return;
        Throttle10Hz.Run<UpdatePlayerCursorPositionCommand>(() =>
        {
            MultiplayerManager.Instance.NetClient.Send(new UpdatePlayerCursorPositionCommand(@event.Player.Id, @event.EventArgs));
        });
    }
    */
}
