using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.Platform.Common.Network;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace MultiplayerMod.Platform.Common.Network.Components;

public class ClientComponent : MultiplayerMonoBehaviour
{

    [InjectDependency]
    private SharedClient client = null!;

    private void Update() => client.Tick();

}
