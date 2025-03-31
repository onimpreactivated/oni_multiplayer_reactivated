namespace MultiplayerMod.ChoreSync.StateMachines;

/// <summary>
/// Info for the create <see cref="Parameter{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="name"></param>
/// <param name="shared"></param>
/// <param name="defaultValue"></param>
public class ParameterInfo<T>(string name, bool shared = true, T defaultValue = default)
{
    /// <summary>
    /// Name for the Parameter
    /// </summary>
    public string Name => name;
    /// <summary>
    /// Type for the Parameter
    /// </summary>
    public Type ValueType => typeof(T);

    /// <summary>
    /// Is Parameter shared to others.
    /// </summary>
    public bool Shared => shared;

    /// <summary>
    /// Default value for <typeparamref name="T"/>
    /// </summary>
    public object DefaultValue => defaultValue;
}
