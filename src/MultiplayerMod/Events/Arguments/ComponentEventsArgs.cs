using System.Reflection;

namespace MultiplayerMod.Events.Arguments;

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
