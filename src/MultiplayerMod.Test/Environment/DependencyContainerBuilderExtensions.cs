using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Core.Events;
using MultiplayerMod.ModRuntime.Context;
using MultiplayerMod.Multiplayer;
using MultiplayerMod.Multiplayer.Chores;
using MultiplayerMod.Multiplayer.Commands.Registry;
using MultiplayerMod.Multiplayer.Objects;
using MultiplayerMod.Multiplayer.Players;
using MultiplayerMod.Multiplayer.StateMachines.Configuration;
using MultiplayerMod.Test.Environment.Network;
using MultiplayerMod.Test.Multiplayer;

namespace MultiplayerMod.Test.Environment;

public static class DependencyContainerBuilderExtensions {

    public static DependencyContainerBuilder AddSystem(this DependencyContainerBuilder builder) => builder
        .AddType<TestRuntime>()
        .AddType<EventDispatcher>()
        .AddType<ExecutionLevelManager>()
        .AddType<ExecutionContextManager>()
        .AddType<MultiplayerGame>()
        .AddType<MultiplayerObjects>()
        .AddType<MultiplayerCommandRegistry>();

    public static DependencyContainerBuilder AddNetworking(this DependencyContainerBuilder builder) => builder
        .AddType<TestMultiplayerServer>()
        .AddType<TestMultiplayerClient>()
        .AddSingleton(new MultiplayerTools.TestPlayerProfileProvider(new PlayerProfile("Test")))
        .AddSingleton(new TestMultiplayerClientId(1));

    public static DependencyContainerBuilder AddStateMachineAndChoreConfigurers(this DependencyContainerBuilder builder) => builder
        .ScanAssembly(
            typeof(IChoreConfigurer).Assembly,
            it => typeof(IChoreConfigurer).IsAssignableFrom(it) || typeof(IStateMachineConfigurer).IsAssignableFrom(it)
        );

}
