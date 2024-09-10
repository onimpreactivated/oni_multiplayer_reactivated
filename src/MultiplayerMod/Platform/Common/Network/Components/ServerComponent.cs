using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.Platform.Common.Network;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace MultiplayerMod.Platform.Common.Network.Components;

public class ServerComponent : MultiplayerMonoBehaviour
{

    [InjectDependency]
    private SharedServer server = null!;

    private void Update() => server.Tick();

}
