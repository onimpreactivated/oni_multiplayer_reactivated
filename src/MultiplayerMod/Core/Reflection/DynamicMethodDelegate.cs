using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Reflection;

public class DynamicMethodDelegate(object target, MethodInfo method) {

    private readonly InvocationDelegate invoke = CreateDelegate(method);
    public object? Invoke(params object?[] arguments) => invoke(target, arguments);

    private static readonly ConditionalWeakTable<MethodInfo, InvocationDelegate> cache = new();

    private static InvocationDelegate CreateDelegate(MethodInfo method) {
        if (cache.TryGetValue(method, out var cachedDelegate))
            return cachedDelegate;

        var parameters = method.GetParameters();
        var invoke = new DynamicMethod(
            name: "_dynamic_invoke",
            returnType: typeof(object),
            parameterTypes: [typeof(object), typeof(object[])],
            owner: typeof(DynamicMethodDelegate)
        );
        var il = invoke.GetILGenerator();

        if (!method.IsStatic)
            il.Emit(OpCodes.Ldarg_0);

        var referencedLocals = new List<(int, Type, LocalBuilder)>();

        for (var i = 0; i < parameters.Length; i++) {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldelem_Ref);

            var type = parameters[i].ParameterType;
            var byRef = type.IsByRef;
            if (byRef)
                type = type.GetElementType()!;
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);

            if (byRef) {
                var local = il.DeclareLocal(type);
                il.Emit(OpCodes.Stloc, local);
                il.Emit(OpCodes.Ldloca, local);
                referencedLocals.Add(new ValueTuple<int, Type, LocalBuilder>(i, type, local));
            }
        }

        il.Emit(method.IsFinal ? OpCodes.Call : OpCodes.Callvirt, method);

        if (method.ReturnType == typeof(void))
            il.Emit(OpCodes.Ldnull);
        else if (method.ReturnType.IsValueType)
            il.Emit(OpCodes.Box, method.ReturnType);

        foreach (var (parameterIndex, type, local) in referencedLocals) {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, parameterIndex);
            il.Emit(OpCodes.Ldloc, local);
            if (type.IsValueType)
                il.Emit(OpCodes.Box, type);
            il.Emit(OpCodes.Stelem_Ref);
        }

        il.Emit(OpCodes.Ret);

        var invocationDelegate = (InvocationDelegate) invoke.CreateDelegate(typeof(InvocationDelegate));
        cache.Add(method, invocationDelegate);

        return invocationDelegate;
    }

    private delegate object? InvocationDelegate(object instance, object?[] args);

}
