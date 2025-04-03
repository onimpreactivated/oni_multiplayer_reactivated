namespace MultiplayerMod.Core.Wrappers;

/// <summary>
/// Boxed Value for Weak Tables
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="value"></param>
public class BoxedValue<T>(T value)
{
    /// <summary>
    /// A Value that parsed into <see cref="BoxedValue{T}"/>
    /// </summary>
    public T Value { get; set; } = value;
}
