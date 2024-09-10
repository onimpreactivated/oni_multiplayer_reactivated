using MultiplayerMod.Network;
using MultiplayerMod.Platform.Common.Network.Messaging;

namespace MultiplayerMod.Platform.Lan.Network;

internal class ClientMessage {
    internal readonly IMultiplayerClientId clientId;
    internal readonly NetworkMessage message;

    public ClientMessage(IMultiplayerClientId clientId, NetworkMessage message) {
        this.clientId = clientId;
        this.message = message;
    }
}
