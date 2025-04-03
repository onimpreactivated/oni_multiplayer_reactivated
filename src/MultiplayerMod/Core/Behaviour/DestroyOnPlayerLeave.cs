using MultiplayerMod.Core.Player;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;

namespace MultiplayerMod.Core.Behaviour;

/// <summary>
/// When <see cref="CorePlayer"/> leaves the object will be destroyed.
/// </summary>
public class DestroyOnPlayerLeave : KMonoBehaviour
{

    [MyCmpReq]
    private readonly PlayerAssigner playerComponent = null!;

    /// <inheritdoc/>
    public override void OnSpawn()
    {
        var player = playerComponent.Player;
        EventManager.SubscribeEvent<PlayerLeftEvent>(OnLeave);
    }

    private void OnLeave(PlayerLeftEvent @event)
    {
        var player = playerComponent.Player;
        if (@event.Player == player)
            DestroyImmediate(gameObject);
    }

    /// <inheritdoc/>
    public override void OnForcedCleanUp() => EventManager.UnsubscribeEvent<PlayerLeftEvent>(OnLeave);
}
