using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Multiplayer.Chores;
using MultiplayerMod.Test.Environment;
using NUnit.Framework;

namespace MultiplayerMod.Test.Multiplayer.Chores;

[TestFixture]
public class ChoresMultiplayerConfigurationTest {

    [Test]
    public void GeneralChoresMustBeSynchronized() {
        var type = typeof(IChoreConfigurer);
        var container = new DependencyContainerBuilder()
            .AddSystem()
            .AddNetworking()
            .ScanAssembly(type.Assembly, it => type.IsAssignableFrom(it))
            .Build();

        var choreTypes = AccessTools.GetTypesFromAssembly(Assembly.GetAssembly(typeof(Chore)))
            .Where(it => typeof(Chore).IsAssignableFrom(it))
            .Where(it => it != typeof(Chore))
            .Where(it => !it.IsGenericType);

        var configuredTypes = container.Get<List<IChoreConfigurer>>().Select(it => it.ChoreType);

        Assert.That(configuredTypes, Is.EquivalentTo(choreTypes));
    }

}
