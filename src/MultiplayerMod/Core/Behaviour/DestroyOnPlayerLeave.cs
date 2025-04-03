using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Arguments.CorePlayerArgs;
using MultiplayerMod.Events.Handlers;

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
        PlayerEvents.PlayerLeft += OnLeave;
    }

    private void OnLeave(CorePlayerLeftArg @event)
    {
        var player = playerComponent.Player;
        if (@event.CorePlayer == player)
            DestroyImmediate(gameObject);
    }

    /// <inheritdoc/>
    public override void OnForcedCleanUp() => PlayerEvents.PlayerLeft -= OnLeave;
}
