namespace MultiplayerMod.Core.Objects;

/// <summary>
/// Stores <see cref="MultiplayerObject"/> with the <see cref="MultiplayerId"/>
/// </summary>
public class MultiplayerObjectsIndex
{

    private Dictionary<object, MultiplayerObject> objects = new(new IdentityEqualityComparer<object>());
    private Dictionary<MultiplayerId, object> instances = [];

    /// <summary>
    /// Gets the <see cref="object"/> that is associated with the <paramref name="key"/>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object this[MultiplayerObject key]
    {
        set
        {
            objects[value] = key;
            instances[key.Id] = value;
        }
    }

    /// <summary>
    /// Gets the <see cref="object"/> that is associated with the <paramref name="key"/>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object this[MultiplayerId key] => instances[key];

    /// <summary>
    /// Removes the value with the specified <paramref name="instance"/> from the <see cref="objects"/> and <see cref="instances"/>
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public bool Remove(object instance)
    {
        if (!objects.TryGetValue(instance, out var @object))
            return false;

        objects.Remove(instance);
        instances.Remove(@object.Id);
        return true;
    }

    /// <summary>
    /// Removes the value with the specified <paramref name="id"/> from the <see cref="objects"/> and <see cref="instances"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Remove(MultiplayerId id)
    {
        if (!instances.TryGetValue(id, out var instance))
            return false;

        objects.Remove(instance);
        instances.Remove(id);
        return true;
    }

    /// <summary>
    /// Gets the <paramref name="object"/> associated with the specified <paramref name="instance"/>
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="object"></param>
    /// <returns></returns>
    public bool TryGetObject(object instance, out MultiplayerObject @object) =>
        objects.TryGetValue(instance, out @object);

    /// <summary>
    /// Gets the <paramref name="instance"/> associated with the specified <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public bool TryGetInstance(MultiplayerId id, out object instance) =>
        instances.TryGetValue(id, out instance);

    /// <summary>
    /// Gets the <see cref="objects"/> as <see cref="IEnumerable{T}"/>
    /// </summary>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<object, MultiplayerObject>> GetEnumerable() => objects;

    /// <summary>
    /// Removes all keys and values from the <see cref="objects"/> and <see cref="instances"/>
    /// </summary>
    /// <param name="force"></param>
    public void Clear(bool force = true)
    {
        if (force)
        {
            ClearIndexes();
            return;
        }

        var survivedObjects = new Dictionary<object, MultiplayerObject>(new IdentityEqualityComparer<object>());
        var survivedInstances = new Dictionary<MultiplayerId, object>();
        foreach (var (instance, @object) in objects)
        {
            if (!@object.Persistent)
                continue;

            survivedObjects[instance] = @object;
            survivedInstances[@object.Id] = instance;
        }
        ClearIndexes();
        objects = survivedObjects;
        instances = survivedInstances;
    }

    private void ClearIndexes()
    {
        objects.Clear();
        instances.Clear();
    }

}
