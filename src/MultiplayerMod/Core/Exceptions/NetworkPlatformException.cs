namespace MultiplayerMod.Core.Exceptions;

/// <summary>
/// Initializes a new instance of the <see cref="NetworkPlatformException"/> class with a specified error message.
/// </summary>
/// <param name="message">The message that describes the error.</param>
public class NetworkPlatformException(string message) : Exception(message);
