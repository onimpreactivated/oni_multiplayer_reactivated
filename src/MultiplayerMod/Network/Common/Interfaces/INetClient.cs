using MultiplayerMod.Commands.NetCommands;

namespace MultiplayerMod.Network.Common.Interfaces;

/// <summary>
/// Interface for Platform Client
/// </summary>
public interface INetClient
{
    /// <summary>
    /// Client's Network Identification
    /// </summary>
    public INetId Id { get; }

    /// <summary>
    /// Client's State
    /// </summary>
    public NetStateClient State { get; }

    /// <summary>
    /// Connect client to <paramref name="endpoint"/>
    /// </summary>
    /// <param name="endpoint"></param>
    public void Connect(IEndPoint endpoint);

    /// <summary>
    /// Disconnect client from Host.
    /// </summary>
    public void Disconnect();

    /// <summary>
    /// Tick
    /// </summary>
    public void Tick();

    /// <summary>
    /// Sending Command to Host and other Clients
    /// </summary>
    /// <param name="command"></param>
    /// <param name="options"></param>
    public void Send(BaseCommandEvent command, MultiplayerCommandOptions options = MultiplayerCommandOptions.None);

    /// <summary>
    /// Action that called when the Client changes <see cref="NetStateClient"/>
    /// </summary>
    public event Action<NetStateClient> StateChanged;

    /// <summary>
    /// Action that called when the Client receives new <see cref="BaseCommandEvent"/>
    /// </summary>
    public event Action<BaseCommandEvent> CommandReceived;
}
