using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Events.MainMenu;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class ExecutionCalls
{
    internal static void OnSinglePlayerModeSelected(SinglePlayerModeSelectedEvent _)
    {
        Debug.Log("Execution set to System");
        ExecutionManager.CurrentLevel = ExecutionLevel.System;
    }

    internal static void OnMultiplayerModeSelected(MultiplayerModeSelectedEvent _)
    {
        Debug.Log("Execution set to Multiplayer");
        ExecutionManager.CurrentLevel = ExecutionLevel.Multiplayer;
    }

    internal static void OnPlayerStateChangedEvent(PlayerStateChangedEvent @event)
    {
        if (MultiplayerManager.Instance.MultiGame.Players.Current == @event.Player && @event.Player.State == PlayerState.Ready)
        {
            Debug.Log("Execution set to Game");
            ExecutionManager.CurrentLevel = ExecutionLevel.Game;
        }

    }

    internal static void OnStopMultiplayer(StopMultiplayerEvent _)
    {
        Debug.Log("Execution set to System");
        ExecutionManager.CurrentLevel = ExecutionLevel.System;
    }
}
