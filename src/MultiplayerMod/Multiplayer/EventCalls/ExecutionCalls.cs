using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Arguments.Common;
using MultiplayerMod.Events.Arguments.CorePlayerArgs;
using MultiplayerMod.Events.Handlers;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class ExecutionCalls : BaseEventCall
{
    public override void Init()
    {
        MainMenuEvents.MultiplayerModeSelected += OnMultiplayerModeSelected;
        MainMenuEvents.SinglePlayerModeSelected += OnSinglePlayerModeSelected;
        PlayerEvents.PlayerStateChanged += OnPlayerStateChangedEvent;
        MultiplayerEvents.MultiplayerStop += OnStopMultiplayer;
    }

    internal static void OnSinglePlayerModeSelected()
    {
        Debug.Log("Execution set to System");
        ExecutionManager.CurrentLevel = ExecutionLevel.System;
    }

    internal static void OnMultiplayerModeSelected(PlayerRoleArg _)
    {
        Debug.Log("Execution set to Multiplayer");
        ExecutionManager.CurrentLevel = ExecutionLevel.Multiplayer;
    }

    internal static void OnPlayerStateChangedEvent(CorePlayerStateChanged @event)
    {
        if (MultiplayerManager.Instance.MultiGame.Players.Current == @event.CorePlayer && @event.State == PlayerState.Ready)
        {
            Debug.Log("Execution set to Game");
            ExecutionManager.CurrentLevel = ExecutionLevel.Game;
        }
    }

    internal static void OnStopMultiplayer()
    {
        Debug.Log("Execution set to System");
        ExecutionManager.CurrentLevel = ExecutionLevel.System;
    }
}
