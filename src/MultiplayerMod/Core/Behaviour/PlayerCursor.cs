using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Arguments.CorePlayerArgs;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Extensions;
using UnityEngine;

namespace MultiplayerMod.Core.Behaviour;

internal class PlayerCursor : KMonoBehaviour
{
    public override void OnSpawn()
    {
        PlayerEvents.PlayerJoined += OnPlayerJoined;
        MultiplayerManager.Instance.MultiGame.Players.ForEach(CreatePlayerCursor);
    }

    private void OnPlayerJoined(CorePlayerArg @event) => CreatePlayerCursor(@event.CorePlayer);

    public override void OnForcedCleanUp() => PlayerEvents.PlayerJoined -= OnPlayerJoined;

    private void CreatePlayerCursor(CorePlayer player)
    {
        if (player == MultiplayerManager.Instance.MultiGame.Players.Current)
            return;
        var canvas = GameScreenManager.Instance.GetParent(GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
        var cursorName = $"{player.Profile.PlayerName}'s cursor";
        var cursor = new GameObject(cursorName) { transform = { parent = canvas.transform } };
        cursor.AddComponent<PlayerAssigner>().Player = player;
        cursor.AddComponent<CursorComponent>();
        cursor.AddComponent<DestroyOnPlayerLeave>();
    }
}
