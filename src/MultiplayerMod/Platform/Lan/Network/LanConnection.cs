using System;

namespace MultiplayerMod.Platform.Lan.Network;

internal class LanConnection : IEquatable<LanConnection> {
    internal readonly LanMultiplayerClientId id;

    internal LanConnection(LanMultiplayerClientId id) {
        this.id = id;
    }

    public bool Equals(LanConnection other) {
        return other.id == id;
    }
}
