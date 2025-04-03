using HarmonyLib;
using MultiplayerMod.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Events;

/// <summary>
/// Managing custom event system
/// </summary>
public static class EventManager
{
    private static readonly Dictionary<Type, HashSet<Delegate>> _events = [];
    private static List<Type> BaseEventDeclaredTypes = [];
    private static readonly object Lock = new();

    /// <summary>
    /// Load assembly and add <see cref="BaseEvent"/> types into <see cref="BaseEventDeclaredTypes"/>
    /// </summary>
    /// <param name="assembly"></param>
    public static void LoadMain(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(BaseEvent)) && !x.IsAbstract))
        {
            Debug.Log($"Loading type from Assembly: {type.FullName}");
            BaseEventDeclaredTypes.Add(type);
        }

        SubscribeAssemblyEvents(assembly);
    }

    /// <summary>
    /// Subscribe all events from <paramref name="assembly"/>
    /// </summary>
    /// <param name="assembly"></param>
    public static void SubscribeAssemblyEvents(Assembly assembly)
    {
        //Debug.Log($"Loading Events from Assembly: {assembly.FullName}");
        foreach (var type in assembly.GetTypes())
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(
                x => x.GetParameters().Length == 1
                && BaseEventDeclaredTypes.Contains(x.GetParameters()[0].ParameterType)
                && x.IsStatic
                && x.GetCustomAttribute<NoAutoSubscribeAttribute>() == null);
            if (!methods.Any())
                continue;
            methods.ForEach(SubscribeEvent);
        }
    }

    /// <summary>
    /// Subscribe desired <paramref name="methodInfo"/> to the event system
    /// </summary>
    /// <param name="methodInfo"></param>
    public static void SubscribeEvent(MethodInfo methodInfo)
    {
        Debug.Log($"Loading Events from methodInfo: {methodInfo.Name}");
        Type param = methodInfo.GetParameters()[0].ParameterType;
        var @delegate = Delegate.CreateDelegate(System.Linq.Expressions.Expression.GetActionType(param), methodInfo);
        SubscribeEvent(param, @delegate);
    }

    /// <summary>
    /// Trigger / Dispatch / Delegate all events that using <paramref name="e"/>
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="e"></param>
    /// <param name="memberName"></param>
    public static void TriggerEvent<TEvent>(TEvent e, [CallerMemberName] string memberName = "") where TEvent : BaseEvent
    {
        Type etype = e.GetType();
        Debug.Log($"TriggerEvent: {etype.Name} {memberName}");
        lock (Lock)
        {
            if (_events.TryGetValue(etype, out var delegates))
            {
                HashSet<Delegate> ToRemove = [];
                // casting ToList will not cause lock issue.
                foreach (var @delegate in delegates.ToList())
                {
                    Debug.Log($"TriggerEvent: {@delegate.Method.FullDescription()}");
                    @delegate.DynamicInvoke(e);
                    //((Action<TEvent>)@delegate)(e);
                    if (@delegate.Method.GetCustomAttribute<UnsubAfterCallAttribute>() != null)
                    {
                        ToRemove.Add(@delegate);
                    }
                }
                // removes all delegates that has UnsubAfterCallAttribute.
                _events[etype].RemoveWhere(ToRemove.Contains);
            }
        }
    }

    /// <summary>
    /// Subscribe to <paramref name="delegate"/> with Type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="delegate"></param>
    public static void SubscribeEvent<T>(Action<T> @delegate) where T : BaseEvent
    {
        Debug.Log($"SubscribeEvent Action: {typeof(T).FullName} | {@delegate.Method.FullDescription()}");
        lock (Lock)
        {
            if (!_events.TryGetValue(typeof(T), out HashSet<Delegate> delegates))
            {
                _events[typeof(T)] = delegates = [];
            }
            delegates.Add(@delegate);
        }
    }

    /// <summary>
    /// Subscribe to <paramref name="delegate"/> with Type <paramref name="baseEventType"/>
    /// </summary>
    /// <param name="baseEventType"></param>
    /// <param name="delegate"></param>
    public static void SubscribeEvent(Type baseEventType, Delegate @delegate)
    {
        Debug.Log($"SubscribeEvent Type: {baseEventType.FullName} | {@delegate.Method.FullDescription()}");
        lock (Lock)
        {
            if (!_events.TryGetValue(baseEventType, out HashSet<Delegate> delegates))
            {
                _events[baseEventType] = delegates = [];
            }
            delegates.Add(@delegate);
        }
    }

    /// <summary>
    /// Unsubcribe from the <paramref name="delegate"/> with Type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="delegate"></param>
    public static void UnsubscribeEvent<T>(Action<T> @delegate) where T : BaseEvent
    {
        Debug.Log($"UnsubscribeEvent Action: {typeof(T).FullName} | {@delegate.Method.FullDescription()}");
        lock (Lock)
        {
            if (_events.TryGetValue(typeof(T), out var delegates))
            {
                delegates.Remove(@delegate);
            }
        }
    }
}
