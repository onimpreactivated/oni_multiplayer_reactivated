namespace MultiplayerMod.Network.Common;

/// <summary>
/// Networking state of the Client
/// </summary>
public enum NetStateClient
{
    /// <summary>
    /// Client received an Error during operation
    /// </summary>
    Error = -1,
    /// <summary>
    /// Client has been disconnected from the Host
    /// </summary>
    Disconnected,
    /// <summary>
    /// Client connecting to Host
    /// </summary>
    Connecting,
    /// <summary>
    /// Client is connected to Host
    /// </summary>
    Connected
}
