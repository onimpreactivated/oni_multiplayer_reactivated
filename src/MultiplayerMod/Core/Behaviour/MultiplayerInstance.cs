using MultiplayerMod.Core.Objects;

namespace MultiplayerMod.Core.Behaviour;

/// <summary>
/// Instance saving objects
/// </summary>
public class MultiplayerInstance : KMonoBehaviour
{
    /// <summary>
    /// List of <see cref="MultiplayerObjects"/> that registered.
    /// </summary>
    public MultiplayerObjects Objects;

    private MultiplayerObject multiplayerObject;

    /// <summary>
    /// Current Multiplayer Id of the object
    /// </summary>
    public MultiplayerId Id => multiplayerObject?.Id;

    /// <summary>
    /// Checks if <see cref="multiplayerObject"/> is not null and its registered in <see cref="Objects"/>
    /// </summary>
    public bool Valid => multiplayerObject != null && Objects.Valid(multiplayerObject);


    /// <inheritdoc/>
    public override void OnCleanUp()
    {
        if (multiplayerObject != null)
            Objects.Remove(multiplayerObject.Id);
    }

    /// <summary>
    /// Register <paramref name="id"/> to <see cref="Objects"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MultiplayerId Register(MultiplayerId id = null)
    {
        multiplayerObject = Objects.Register(gameObject, id);
        return multiplayerObject.Id;
    }

    /// <summary>
    /// Redirect current Instance to this new one.
    /// </summary>
    /// <param name="destination"></param>
    public void Redirect(MultiplayerInstance destination)
    {
        if (multiplayerObject == null)
            return;
        destination.multiplayerObject = multiplayerObject;
        Objects.Register(destination.gameObject, multiplayerObject.Id, multiplayerObject.Persistent);
        multiplayerObject = null;
    }
}
