using UnityEngine;

namespace MultiplayerMod.Core.Unity;

/// <summary>
/// Static class for managing <see cref="FakeUnityObject"/> and <see cref="GameObject"/>
/// </summary>
public static class UnityObjectManager
{

    private static readonly IntPtr unityPlayerNullObject;

    static UnityObjectManager()
    {
        unityPlayerNullObject = FakeUnityObject.CreateEmpty();
    }

    /// <summary>
    /// Create a new <see cref="GameObject"/> with <typeparamref name="T"/> as a <see cref="Component"/> and using <see cref="UnityEngine.Object.DontDestroyOnLoad"/> to make it static
    /// </summary>
    /// <typeparam name="T">Any <see cref="Component"/></typeparam>
    /// <returns></returns>
    public static GameObject CreateStaticWithComponent<T>() where T : Component =>
        CreateWithComponents(false, typeof(T));

    /// <summary>
    /// Create a new <see cref="GameObject"/> with <typeparamref name="T1"/>, <typeparamref name="T2"/> as a <see cref="Component"/>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static GameObject CreateWithComponent<T1, T2>() where T1 : Component where T2 : Component =>
        CreateWithComponents(true, typeof(T1), typeof(T2));

    private static GameObject CreateWithComponents(bool destroyOnLoad, params Type[] components)
    {
        var gameObject = new GameObject(null, components);
        if (!destroyOnLoad)
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        return gameObject;
    }

    /// <summary>
    /// Destroy the <paramref name="gameObject"/>
    /// </summary>
    /// <param name="gameObject"></param>
    public static void Destroy(GameObject gameObject)
    {
        UnityEngine.Object.Destroy(gameObject);
    }

    /// <summary>
    /// Create a <see cref="FakeUnityObject"/>
    /// </summary>
    /// <typeparam name="T">Any <see cref="MonoBehaviour"/></typeparam>
    /// <returns>New <typeparamref name="T"/></returns>
    public static T CreateStub<T>() where T : MonoBehaviour, new() =>
        new()
        {
            m_CachedPtr = unityPlayerNullObject // Support for != and == for Unity objects
        };

}
