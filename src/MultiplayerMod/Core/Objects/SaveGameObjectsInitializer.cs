using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Core.Objects;

/// <summary>
/// Initializer for Prefabs, Chores and other game object to <paramref name="objects"/>
/// </summary>
/// <param name="objects"></param>
public class SaveGameObjectsInitializer(MultiplayerObjects objects)
{
    /// <summary>
    /// Inizialize Prefabs and Chores to <see cref="MultiplayerObjects"/>
    /// </summary>
    public void Initialize()
    {
        AddPrefabs();
        AddChores();
        Debug.Log("SaveGameObjectsInitializer.Initialize");
    }

    private void AddPrefabs()
    {
        var kPrefabIds = KPrefabIDTracker.Get().prefabIdMap.Values;
        foreach (var kPrefabId in kPrefabIds)
        {
            if (kPrefabId == null)
                continue;

            var gameObject = kPrefabId.gameObject;
            var instance = gameObject.GetComponent<MultiplayerInstance>();
            if (instance.Valid)
                continue;
            instance.Register(new MultiplayerId(InternalMultiplayerIdType.KPrefabId, kPrefabId.InstanceID));
        }
    }

    private void AddChores()
    {
        UnityEngine.Object.FindObjectsOfType<ChoreProvider>()
            .SelectMany(it => it.choreWorldMap)
            .SelectMany(it => it.Value)
            .Where(it => !it.gameObject.GetComponent<MultiplayerInstance>().Valid)
            .ForEach(it => { objects.Register(it, new MultiplayerId(InternalMultiplayerIdType.Chore, it.id)); });
    }
}
