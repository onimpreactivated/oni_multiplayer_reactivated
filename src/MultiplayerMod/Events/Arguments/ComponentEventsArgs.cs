using System.Reflection;

namespace MultiplayerMod.Events.Arguments;

/// <summary>
/// Arguments supplied for <see cref="Others.ComponentMethodCalled"/>
/// </summary>
/// <param name="component"></param>
/// <param name="method"></param>
/// <param name="args"></param>
public class ComponentEventsArgs(KMonoBehaviour component, MethodBase method, object[] args)
{
    /// <summary>
    /// Component thats called.
    /// </summary>
    public KMonoBehaviour Component => component;

    /// <summary>
    /// The original method
    /// </summary>
    public MethodBase Method => method;

    /// <summary>
    /// Argument the method to supply
    /// </summary>
    public object[] Args => args;
}
