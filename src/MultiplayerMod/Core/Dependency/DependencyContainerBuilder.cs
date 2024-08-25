using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MultiplayerMod.Core.Dependency;

public class DependencyContainerBuilder {

    private readonly List<(Assembly, Func<Type, bool>)> assemblies = new();
    private readonly List<Type> includeTypes = new();
    private readonly List<object> singletons = new();

    public event Action<DependencyContainer>? ContainerCreated;

    public DependencyContainerBuilder ScanAssembly(Assembly assembly) => ScanAssembly(assembly, _ => true);

    public DependencyContainerBuilder ScanAssembly(Assembly assembly, params string[] namespaces) => ScanAssembly(
        assembly,
        type => namespaces.Any(name => type.Namespace?.StartsWith(name) == true)
    );

    public DependencyContainerBuilder ScanAssembly(Assembly assembly, Func<Type, bool> predicate) {
        assemblies.Add((assembly, predicate));
        return this;
    }

    public DependencyContainerBuilder AddType<T>() => AddType(typeof(T));

    public DependencyContainerBuilder AddType(Type type) {
        includeTypes.Add(type);
        return this;
    }

    public DependencyContainerBuilder AddSingleton(object singleton) {
        singletons.Add(singleton);
        return this;
    }

    public DependencyContainer Build() {
        var container = new DependencyContainer();
        container.RegisterSingleton(container);
        singletons.ForEach(it => container.RegisterSingleton(it));
        var types = assemblies.SelectMany(it => it.Item1.DefinedTypes.Where(it.Item2))
            .Where(it => it.GetCustomAttribute<DependencyAttribute>() != null)
            .Union(includeTypes);
        foreach (var type in types) {
            var attribute = type.GetCustomAttribute<DependencyAttribute>();
            var dependencyInfo = new DependencyInfo(attribute?.Name ?? type.FullName!, type, attribute?.Lazy ?? false);
            container.Register(dependencyInfo);
        }
        container.PreInstantiate();
        ContainerCreated?.Invoke(container);
        return container;
    }

}
