using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Core.Exceptions;

/// <summary>
/// Initializes a new instance of the <see cref="ObjectNotFoundException"/> class with a specified error message.
/// </summary>
/// <param name="reference">The message that describes the error.</param>
public class ObjectNotFoundException(IResolver reference) : Exception
{
    /// <summary>
    /// Getting the Reference Type from Exception
    /// </summary>
    public IResolver Reference { get; } = reference;
}
