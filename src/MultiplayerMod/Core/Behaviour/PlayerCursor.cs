using MultiplayerMod.Core.Player;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Extensions;
using UnityEngine;

namespace MultiplayerMod.Core.Behaviour;

internal class PlayerCursor : KMonoBehaviour
{
    public override void OnSpawn()
    {
        EventManager.SubscribeEvent<PlayerJoinedEvent>(OnPlayerJoined);
        MultiplayerManager.Instance.MultiGame.Players.ForEach(CreatePlayerCursor);
    }

    private void OnPlayerJoined(PlayerJoinedEvent @event) => CreatePlayerCursor(@event.Player);

    public override void OnForcedCleanUp() => EventManager.UnsubscribeEvent<PlayerJoinedEvent>(OnPlayerJoined);

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
