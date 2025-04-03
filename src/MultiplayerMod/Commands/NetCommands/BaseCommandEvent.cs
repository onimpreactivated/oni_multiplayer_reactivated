using MultiplayerMod.Events;
using MultiplayerMod.Network.Common.Interfaces;

namespace MultiplayerMod.Commands.NetCommands;


/// <summary>
/// Base event for handling Commands
/// </summary>
[Serializable]
public abstract class BaseCommandEvent : BaseEvent
{
    /// <summary>
    /// Identification for the Command
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// 
    /// </summary>
    public INetId ClientId { get; internal set; }

    /// <inheritdoc/>
    public override string ToString() => $"Command [{Id:N}] {GetType().Name}";
}
