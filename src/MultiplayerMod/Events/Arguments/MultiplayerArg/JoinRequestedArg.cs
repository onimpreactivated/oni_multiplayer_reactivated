using MultiplayerMod.Network.Common.Interfaces;

namespace MultiplayerMod.Events.Arguments.MultiplayerArg;

public class JoinRequestedArg(IEndPoint endpoint, string hostName) : EventArgs
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
