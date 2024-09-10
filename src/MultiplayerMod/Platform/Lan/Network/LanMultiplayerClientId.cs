using MultiplayerMod.Network;
using System;

namespace MultiplayerMod.Platform.Lan.Network;

[Serializable]
public record LanMultiplayerClientId : IMultiplayerClientId {
    public readonly string Id;

    public LanMultiplayerClientId() : this(Guid.NewGuid().ToString()) { }

    public LanMultiplayerClientId(string id) {
        Id = id;
    }

    public bool Equals(IMultiplayerClientId other) {
        return other is LanMultiplayerClientId player && player.Equals(this);
    }

    public override string ToString() {
        return Id;
    }
}
