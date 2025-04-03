using MultiplayerMod.Core.Player;
using MultiplayerMod.Network.Common.Interfaces;

namespace MultiplayerMod.Events.Others;

/// <summary>
/// Event that is exists to join to the <paramref name="endpoint"/>
/// </summary>
/// <param name="endpoint"></param>
/// <param name="hostName"></param>
public class MultiplayerJoinRequestedEvent(IEndPoint endpoint, string hostName) : BaseEvent
{
    /// <summary>
    /// Endpoint the user must join to.
    /// </summary>
    public IEndPoint Endpoint => endpoint;

    /// <summary>
    /// Name of the server.
    /// </summary>
    public string HostName => hostName;
}

/// <summary>
/// Initialize <paramref name="profile"/> in server that the <paramref name="clientId"/> is controlling
/// </summary>
/// <param name="clientId"></param>
/// <param name="profile"></param>
public class ClientInitializationRequestEvent(INetId clientId, PlayerProfile profile) : BaseEvent
{
    /// <summary>
    /// ClientId that the server control
    /// </summary>
    public INetId ClientId => clientId;

    /// <summary>
    /// The profile <see cref="ClientId"/> has.
    /// </summary>
    public PlayerProfile Profile => profile;
}
