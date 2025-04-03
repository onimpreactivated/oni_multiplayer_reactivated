using MultiplayerMod.Commands.NetCommands;

namespace MultiplayerMod.Network.Common.Interfaces;

/// <summary>
/// Interface for Plaform Server
/// </summary>
public interface INetServer
{
    /// <summary>
    /// Current <see cref="NetStateServer"/> of the Server
    /// </summary>
    public NetStateServer State { get; }

    /// <summary>
    /// Current <see cref="IEndPoint"/> of the Server
    /// </summary>
    public IEndPoint Endpoint { get; }

    /// <summary>
    /// <see cref="List{T}"/> of Connected <see cref="INetId"/>'s
    /// </summary>
    public List<INetId> Clients { get; }

    /// <summary>
    /// Start hosting the Server
    /// </summary>
    public void Start();

    /// <summary>
    /// Stop hosting the Server
    /// </summary>
    public void Stop();

    /// <summary>
    /// Tick
    /// </summary>
    public void Tick();

    /// <summary>
    /// Send <paramref name="command"/> to <paramref name="clientId"/>
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="command"></param>
    public void Send(INetId clientId, BaseCommandEvent command);

    /// <summary>
    /// Send <paramref name="command"/> to all <see cref="Clients"/> with <see cref="MultiplayerCommandOptions"/> as <paramref name="options"/>
    /// </summary>
    /// <param name="command"></param>
    /// <param name="options"></param>
    public void Send(BaseCommandEvent command, MultiplayerCommandOptions options = MultiplayerCommandOptions.None);

    /// <summary>
    /// Action that called when the Server changes it's <see cref="NetStateServer"/>
    /// </summary>
    public event Action<NetStateServer> StateChanged;

    /// <summary>
    /// Action that called when new Client connected as <see cref="INetId"/>
    /// </summary>
    public event Action<INetId> ClientConnected;

    /// <summary>
    /// Action that called when new Client disconnected as <see cref="INetId"/>
    /// </summary>
    public event Action<INetId> ClientDisconnected;

    /// <summary>
    /// Action that called when the Server receives new <see cref="BaseCommandEvent"/> from <see cref="INetId"/>
    /// </summary>
    public event Action<INetId, BaseCommandEvent> CommandReceived;
}
