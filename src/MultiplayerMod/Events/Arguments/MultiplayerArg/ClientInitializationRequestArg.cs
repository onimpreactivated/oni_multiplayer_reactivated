using MultiplayerMod.Core.Player;
using MultiplayerMod.Network.Common.Interfaces;

namespace MultiplayerMod.Events.Arguments.MultiplayerArg;

public class ClientInitializationRequestArg(INetId clientId, PlayerProfile profile) : EventArgs
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
