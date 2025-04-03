using TMPro;
using UnityEngine;

namespace MultiplayerMod.Multiplayer.UI.Overlays;

/// <summary>
/// Status overlay. Showing the default loading screen with custom message
/// </summary>
public class MultiplayerStatusOverlay
{

    /// <summary>
    /// Text displayed on overlay
    /// </summary>
    public static string Text
    {
        get => overlay?.text ?? "";
        set
        {
            if (overlay == null)
                return;

            overlay.text = value;
            overlay.textComponent.text = value;
        }
    }

    private LocText textComponent = null!;
    private string text = "";

    private RectTransform rect = null!;

    // ReSharper disable once InconsistentNaming
    private Func<float> GetScale = null!;

    private static MultiplayerStatusOverlay overlay;

    private MultiplayerStatusOverlay()
    {
        //SceneManager.sceneLoaded += OnPostLoadScene;
        ScreenResize.Instance.OnResize += OnResize;
        CreateOverlay();
    }

    private void CreateOverlay()
    {
        LoadingOverlay.Load(() => { Debug.Log("MultiplayerStatusOverlay.CreateOverlay.Load"); });
        textComponent = LoadingOverlay.instance.GetComponentInChildren<LocText>();
        textComponent.alignment = TextAlignmentOptions.Top;
        textComponent.margin = new Vector4(0, -21.0f, 0, 0);
        textComponent.text = text;

        GetScale = LoadingOverlay.instance.GetComponentInParent<KCanvasScaler>().GetCanvasScale;
        rect = textComponent.gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(Screen.width / GetScale(), 0);
    }

    private void OnResize() => rect.sizeDelta = new Vector2(Screen.width / GetScale(), 0);

    private void Dispose()
    {
        //SceneManager.sceneLoaded -= OnPostLoadScene;
        ScreenResize.Instance.OnResize -= OnResize;
        LoadingOverlay.Clear();
    }

    //private void OnPostLoadScene(Scene scene, LoadSceneMode mode) =>  CreateOverlay();

    /// <summary>
    /// Show overlay with the specific <paramref name="text"/>
    /// </summary>
    /// <param name="text"></param>
    public static void Show(string text)
    {
        overlay ??= new MultiplayerStatusOverlay();
        Text = text;
    }

    /// <summary>
    /// Close overlay.
    /// </summary>
    public static void Close()
    {
        overlay?.Dispose();
        overlay = null;
    }

}
