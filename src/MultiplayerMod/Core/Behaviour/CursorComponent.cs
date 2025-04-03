using MultiplayerMod.Events;
using MultiplayerMod.Events.Others;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MultiplayerMod.Core.Behaviour;

/// <summary>
/// Creates a cursor component
/// </summary>
public class CursorComponent : KMonoBehaviour
{
    // Perhaps we should provide a setting to enable this feature later on.
    private const bool ENABLE_CURSORS_RELATIVE_TO_GUI = false;

    [MyCmpAdd]
    private readonly Canvas canvas = null!;

    [MyCmpReq]
    private readonly PlayerAssigner assignedPlayer = null!;

    private Camera camera = null!;
    private Image cursorImage = null!;
    private TextMeshProUGUI cursorText = null!;

    private readonly SmoothCursor worldCursor = new();
    private readonly SmoothCursor screenCursor = new();

    private string playerName;
    private string screenName;
    private string screenTypeName;

    /// <inheritdoc/>
    public override void OnSpawn()
    {
        camera = GameScreenManager.Instance.GetCamera(GameScreenManager.UIRenderTarget.ScreenSpaceCamera);
        var cursorTexture = Assets.GetTexture("cursor_arrow");
        var cursor = new GameObject(name);
        cursorImage = CreateCursorImage(cursor, cursorTexture);
        cursorText = CreateCursorText(cursor, new Vector3(cursorTexture.width, -cursorTexture.height, 0));
        cursor.transform.SetParent(transform, false);
        gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
        playerName = assignedPlayer.Player.Profile.PlayerName;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 100;
        EventManager.SubscribeEvent<PlayerCursorPositionUpdatedEvent>(OnPlayerCursorPositionUpdated);
    }

    private Image CreateCursorImage(GameObject parent, Texture2D cursorTexture)
    {
        var imageGameObject = new GameObject(name) { transform = { parent = parent.transform } };
        var rectTransform = imageGameObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(cursorTexture.width, cursorTexture.height);
        rectTransform.pivot = new Vector2(0, 1); // Align to top left corner.
        var imageComponent = imageGameObject.AddComponent<Image>();
        imageComponent.sprite = Sprite.Create(
            cursorTexture,
            new Rect(0, 0, cursorTexture.width, cursorTexture.height),
            Vector2.zero
        );
        imageComponent.raycastTarget = false;
        return imageComponent;
    }

    private TextMeshProUGUI CreateCursorText(GameObject parent, Vector3 offset)
    {
        var textGameObject = new GameObject(name) { transform = { parent = parent.transform } };
        var rectTransform = textGameObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        rectTransform.pivot = new Vector2(0, 1); // Align to top left corner.
        rectTransform.position = offset;
        var textComponent = textGameObject.AddComponent<TextMeshProUGUI>();
        textComponent.fontSize = 14;
        textComponent.font = Localization.FontAsset;
        textComponent.color = Color.white;
        textComponent.raycastTarget = false;
        textComponent.enableWordWrapping = false;
        return textComponent;
    }

    /// <inheritdoc/>
    public override void OnForcedCleanUp() => EventManager.UnsubscribeEvent<PlayerCursorPositionUpdatedEvent>(OnPlayerCursorPositionUpdated);

    private void OnPlayerCursorPositionUpdated(PlayerCursorPositionUpdatedEvent @event)
    {
        if (@event.Player != assignedPlayer.Player)
            return;
        Debug.Log("OnPlayerCursorPositionUpdated!");
        var args = @event.EventArgs;

        screenName = args.ScreenName;
        screenTypeName = args.ScreenTypeName;
        worldCursor.Trace(args.Position);
        screenCursor.Trace(args.PositionWithinScreen);
    }

    internal void Update()
    {
        var screenStack = KScreenManager.Instance.screenStack;
        var playerScreen = screenStack.FirstOrDefault(screen => screen.GetType().ToString() == screenTypeName);

        // If we see a screen where other player is - show cursor within that screen.
        var showScreenOrWorldCursor = screenCursor.CurrentPosition != null && (playerScreen?.isActive ?? false);
        if (showScreenOrWorldCursor && ENABLE_CURSORS_RELATIVE_TO_GUI)
        {
            if (screenCursor.CurrentPosition != null)
                transform.position = ScreenToWorld(playerScreen!, screenCursor.CurrentPosition.Value);
        }
        else
        {
            if (worldCursor.CurrentPosition != null)
                transform.position = camera.WorldToScreenPoint(worldCursor.CurrentPosition.Value);
        }

        var screenUnderCursor = FindScreenUnderCursor(transform.position);
        var showScreenName = screenUnderCursor?.GetType().ToString() != screenTypeName;

        cursorText.text = playerName + (showScreenName ? $" ({screenName ?? "World"})" : "");
        cursorImage.color = cursorText.color = showScreenName ? new Color(1, 1, 1, 0.5f) : Color.white;
    }

    private KScreen FindScreenUnderCursor(Vector2 cursor)
    {
        var eventSystem = UnityEngine.EventSystems.EventSystem.current;
        if (eventSystem == null)
            return null;

        var eventData = new PointerEventData(eventSystem) { position = cursor };
        var results = new List<RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, results);
        if (results.Count == 0)
            return null;

        var raycastResult = results[0].gameObject;
        var screenStack = KScreenManager.Instance.screenStack;

        return screenStack.FirstOrDefault(screen => IsParentOf(screen.gameObject, raycastResult));
    }

    private static bool IsParentOf(GameObject potentialParent, GameObject potentialChild)
    {
        var current = potentialChild.transform;
        while (current != null)
        {
            if (current.gameObject == potentialParent)
                return true;

            current = current.parent;
        }
        return false;
    }

    private static Vector3 ScreenToWorld(KScreen screen, Vector3 pos)
    {
        var screenRectTransform = screen.transform as RectTransform;
        if (screenRectTransform == null)
            return Vector3.zero;

        var position = screenRectTransform.position;
        var rect = screenRectTransform.rect;

        return new Vector2(position.x + pos.x * rect.width, position.y + pos.y * rect.height);
    }

    private class SmoothCursor
    {

        private class TimedCursor(Vector2? position, long time)
        {
            public Vector2? Position => position;
            public long Time => time;
        }

        private TimedCursor previous;
        private TimedCursor current;

        public SmoothCursor()
        {
            var ticks = System.DateTime.Now.Ticks;
            previous = new TimedCursor(null, ticks);
            current = new TimedCursor(null, ticks);
        }

        public void Trace(Vector2? position)
        {
            previous = current;
            current = new TimedCursor(position, System.DateTime.Now.Ticks);
        }

        public Vector3? CurrentPosition
        {
            get
            {
                if (previous.Position == null)
                    return current.Position;

                if (current.Position == null)
                    return null;

                float updateDelta = current.Time - previous.Time;
                var timeDiff = (System.DateTime.Now.Ticks - current.Time) / updateDelta;
                return Vector2.Lerp((Vector2) previous.Position, (Vector2) current.Position, timeDiff);
            }
        }

    }
}
