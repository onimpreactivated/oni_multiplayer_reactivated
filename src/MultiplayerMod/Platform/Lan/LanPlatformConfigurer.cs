using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Core.Unity;
using MultiplayerMod.ModRuntime.Loader;

namespace MultiplayerMod.Platform.Lan;

[UsedImplicitly]
[ModComponentOrder(ModComponentOrder.Platform)]
public class LanPlatformConfigurer : IModComponentConfigurer {

    public void Configure(DependencyContainerBuilder builder) {
        builder.ContainerCreated += _ => UnityObject.CreateStaticWithComponent<LanJoinRequestComponent>();
    }

}
