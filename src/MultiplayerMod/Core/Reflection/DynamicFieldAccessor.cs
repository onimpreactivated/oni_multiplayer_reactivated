using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Reflection;

public class DynamicFieldAccessor<T>(object instance, FieldInfo field) {

    private readonly DynamicFieldAccessor accessor = new(instance, field);

    public T? Get() => (T?) accessor.Get();

    public void Set(T? value) => accessor.Set(value);

}

public class DynamicFieldAccessor(object instance, FieldInfo field) {

    private static readonly ConditionalWeakTable<FieldInfo, FieldDelegates> cache = new();

    private readonly FieldDelegates delegates = CreateFieldAccessor(field);

    public object? Get() => delegates.Getter(instance);
    public void Set(object? value) => delegates.Setter(instance, value);

    private static FieldDelegates CreateFieldAccessor(FieldInfo field) {
        if (cache.TryGetValue(field, out var cachedDelegates))
            return cachedDelegates;

        var delegates = new FieldDelegates(CreateGetter(field), CreateSetter(field));
        cache.Add(field, delegates);
        return delegates;
    }

    private static Getter CreateGetter(FieldInfo field) {
        var invoke = new DynamicMethod(
            name: "_dynamic_getter",
            returnType: typeof(object),
            parameterTypes: [typeof(object)],
            owner: typeof(DynamicFieldAccessor)
        );
        var il = invoke.GetILGenerator();

        if (field.IsStatic) {
            il.Emit(OpCodes.Ldsfld, field);
        } else {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
        }
        if (field.FieldType.IsValueType)
            il.Emit(OpCodes.Box, field.FieldType);
        il.Emit(OpCodes.Ret);

        return (Getter) invoke.CreateDelegate(typeof(Getter));
    }

    private static Setter CreateSetter(FieldInfo field) {
        var invoke = new DynamicMethod(
            name: "_dynamic_setter",
            returnType: typeof(void),
            parameterTypes: [typeof(object), typeof(object)],
            owner: typeof(DynamicFieldAccessor)
        );
        var il = invoke.GetILGenerator();

        if (!field.IsStatic)
            il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        if (field.FieldType.IsValueType)
            il.Emit(OpCodes.Unbox_Any, field.FieldType);
        il.Emit(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
        il.Emit(OpCodes.Ret);

        return (Setter) invoke.CreateDelegate(typeof(Setter));
    }

    private delegate object? Getter(object instance);

    private delegate void Setter(object instance, object? value);

    private class FieldDelegates(Getter getter, Setter setter) {
        public readonly Getter Getter = getter;
        public readonly Setter Setter = setter;
    }

}
