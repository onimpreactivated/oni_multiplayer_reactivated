using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events;
using MultiplayerMod.Network.Common.Interfaces;

namespace MultiplayerMod.Multiplayer.Controllers;

/// <summary>
/// Server and Client Command receive hooks
/// </summary>
public class MPCommandController
{
    /// <summary>
    /// Register event callers <see cref="INetServer.CommandReceived"/> and <see cref="INetClient.CommandReceived"/>
    /// </summary>
    public static void Registers()
    {
        MultiplayerManager.Instance.NetServer.CommandReceived += OnServerReceivedCommand;

        MultiplayerManager.Instance.NetClient.CommandReceived += OnClientReceivedCommand;

        Debug.Log("MPCommandController Registered");
    }

    [NoAutoSubscribe]
    private static void OnClientReceivedCommand(BaseCommandEvent command)
    {
        Debug.Log("OnClientReceivedCommand Received command: " + command);
        EventManager.TriggerEvent(command);
    }

    private static void OnServerReceivedCommand(INetId clientId, BaseCommandEvent command)
    {
        Debug.Log("OnServerReceivedCommand Received command: " + command);
        command.ClientId = clientId;
        EventManager.TriggerEvent(command);
    }
}
