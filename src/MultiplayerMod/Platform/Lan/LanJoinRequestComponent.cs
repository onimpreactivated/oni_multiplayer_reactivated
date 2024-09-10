using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Core.Events;
using MultiplayerMod.Core.Unity;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace MultiplayerMod.Platform.Lan;

public class LanJoinRequestComponent : MultiplayerMonoBehaviour {
    public static LanJoinRequestComponent? instance;

    [InjectDependency]
    public EventDispatcher eventDispatcher = null!;

    LanJoinRequestComponent() {
        instance = this;
    }

}
