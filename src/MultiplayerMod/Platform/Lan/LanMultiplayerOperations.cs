using MultiplayerMod.Multiplayer;
using MultiplayerMod.Multiplayer.Players.Events;
using MultiplayerMod.Platform.Lan.Network;
using MultiplayerMod.Platform.Steam.Network;

namespace MultiplayerMod.Platform.Lan;

internal class LanMultiplayerOperations : IMultiplayerOperations {
    public void Join() {
        LanJoinRequestComponent.instance?.eventDispatcher.Dispatch(new MultiplayerJoinRequestedEvent(new LanServerEndpoint(), LanConfiguration.instance.displayName));
    }
}
