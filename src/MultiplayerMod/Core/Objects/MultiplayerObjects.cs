using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;

namespace MultiplayerMod.Core.Objects;

/// <summary>
/// Stores <see cref="MultiplayerObjectsIndex"/> and handling events for object existence and creation.
/// </summary>
public class MultiplayerObjects
{

    private readonly MultiplayerObjectsIndex index = new();
    private int generation;

    /// <summary>
    /// Initialize <see cref="MultiplayerObjects"/> and subscribe to many events
    /// </summary>
    public MultiplayerObjects()
    {
        EventManager.SubscribeEvent<GameObjectCreated>(CreateObjects_Event);
        EventManager.SubscribeEvent<WorldLoadingEvent>(_ => Clear());
        EventManager.SubscribeEvent<GameQuitEvent>(_ => Clear());
        var initializer = new SaveGameObjectsInitializer(this);
        EventManager.SubscribeEvent<WorldSyncEvent>(_ =>
        {
            Clear(force: false);
            initializer.Initialize();
        });
        EventManager.SubscribeEvent<GameReadyEvent>(_ => initializer.Initialize());
    }

    /// <summary>
    /// Checks if the <paramref name="object"/> is Persistent and in the same generation.
    /// </summary>
    /// <param name="object"></param>
    /// <returns></returns>
    public bool Valid(MultiplayerObject @object) => @object.Persistent || @object.Generation == generation;

    /// <summary>
    /// Register <paramref name="instance"/> as a new <see cref="MultiplayerObject"/>
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="multiplayerId"></param>
    /// <param name="persistent"></param>
    /// <returns></returns>
    public MultiplayerObject Register(object instance, MultiplayerId multiplayerId = null, bool persistent = false)
    {
        Debug.Log("MultiplayerObjects Register " + instance);
        MultiplayerObject @object = new(multiplayerId ?? new MultiplayerId(Guid.NewGuid()), generation, persistent);
        index[@object] = instance;
        return @object;
    }

    private void Clear(bool force = true)
    {
        Debug.Log("MultiplayerObjects Clear");
        index.Clear(force);
        generation++;
    }

    /// <inheritdoc/>
    public bool Remove(MultiplayerId id) => index.Remove(id);

    /// <inheritdoc/>
    public bool RemoveObject(object instance)
    {
        Debug.Log("MultiplayerObjects RemoveObject " + instance);
        return index.Remove(instance);
    }

    /// <summary>
    /// Get the <typeparamref name="T"/> with <paramref name="id"/> from <see cref="index"/>
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>default of <typeparamref name="T"/> or the retrived <typeparamref name="T"/></returns>
    public T Get<T>(MultiplayerId id) => !index.TryGetInstance(id, out var instance) ? default : (T) instance!;

    /// <summary>
    /// Get the <see cref="MultiplayerObject"/> with <paramref name="instance"/> from <see cref="index"/>
    /// </summary>
    /// <param name="instance"></param>
    /// <returns>null or the retrived <see cref="MultiplayerObject"/></returns>
    public MultiplayerObject Get(object instance) => !index.TryGetObject(instance, out var @object) ? null : @object;

    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<object, MultiplayerObject>> GetEnumerable() => index.GetEnumerable();


    private void CreateObjects_Event(GameObjectCreated gameObjectCreated)
    {
        gameObjectCreated.GameObject.AddComponent<MultiplayerInstance>().Objects = this;
        gameObjectCreated.GameObject.AddComponent<GridObject>();
    }
}
