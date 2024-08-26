using JetBrains.Annotations;
using MultiplayerMod.Core.Dependency;
using NUnit.Framework;

namespace MultiplayerMod.Test.Core.Dependency;

[TestFixture]
public class DependencyContainerBuilderTests {

    [Test]
    public void ContainerReferencesAreAvailable() {
        var container = new DependencyContainerBuilder().Build();
        var containerByInterface = container.Get<IDependencyContainer>();
        var injector = container.Get<IDependencyInjector>();
        var self = container.Get<DependencyContainer>();
        Assert.AreSame(expected: container, actual: self);
        Assert.AreSame(expected: container, actual: containerByInterface);
        Assert.AreSame(expected: container, actual: injector);
    }

    [Test]
    public void ManuallyRegisteredDependencyIsAvailable() {
        var container = new DependencyContainerBuilder()
            .AddType<ManualDependency>()
            .Build();
        Assert.DoesNotThrow(() => container.Get<ManualDependency>());
    }

    [Test]
    public void ManuallyRegisteredSingletonIsAvailable() {
        var singleton = new SingletonDependency();
        var container = new DependencyContainerBuilder()
            .AddSingleton(singleton)
            .AddType<SingletonAccessor>()
            .Build();
        Assert.AreSame(expected: singleton, actual: container.Get<SingletonAccessor>().Singleton);
    }

    [Test]
    public void AssemblyNamespaceDependenciesAreAvailable() {
        var container = new DependencyContainerBuilder()
            .ScanAssembly(GetType().Assembly, "MultiplayerMod.Test.Core.Dependency")
            .Build();
        Assert.DoesNotThrow(() => container.Get<DependencyA>());
        Assert.DoesNotThrow(() => container.Get<DependencyB>());
    }

    [Test]
    public void AssemblyFilteredDependenciesAreAvailable() {
        var container = new DependencyContainerBuilder()
            .ScanAssembly(GetType().Assembly, it => typeof(IDependencyA).IsAssignableFrom(it))
            .Build();
        Assert.DoesNotThrow(() => container.Get<DependencyA>());
        Assert.Throws<MissingDependencyException>(() => container.Get<DependencyB>());
    }

    [UsedImplicitly]
    public class ManualDependency;

    public interface IDependencyA;

    [Dependency, UsedImplicitly]
    public class DependencyA : IDependencyA;

    [Dependency, UsedImplicitly]
    public class DependencyB;

    [UsedImplicitly]
    public class SingletonAccessor(SingletonDependency singleton) {
        public SingletonDependency Singleton { get; } = singleton;
    }

    public class SingletonDependency;

}
