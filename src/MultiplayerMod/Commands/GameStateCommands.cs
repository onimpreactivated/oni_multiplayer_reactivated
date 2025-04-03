using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Handlers;
using MultiplayerMod.Network.Common;
using MultiplayerMod.Patches.ScreenPatches;

namespace MultiplayerMod.Commands;

internal class GameStateCommands
{
    internal static void PauseGame_Event(PauseGameCommand _)
    {
        // fix doesnt initialize the game itself
        if (SpeedControlScreen.Instance == null)
            return;
        if (!SpeedControlScreen.Instance.IsPaused)
            SpeedControlScreen.Instance.Pause();
    }
    internal static void ResumeGame_Event(ResumeGameCommand _)
    {
        if (TryResume(MultiplayerManager.Instance.MultiGame))
            return;
        MultiplayerEvents.PlayersReady += OnPlayersReady;
        if (SpeedControlScreen.Instance == null)
            return;
        if (!SpeedControlScreen.Instance.IsPaused)
            SpeedControlScreen.Instance.Pause();
    }

    internal static void ChangeGameSpeed_Event(ChangeGameSpeed gameSpeed)
    {
        // fix doesnt initialize the game itself
        if (SpeedControlScreen.Instance == null)
            return;
        SpeedControlScreenPatch.IsSpeedSetByCommand = true;
        SpeedControlScreen.Instance.SetSpeed(gameSpeed.Speed);
    }

    internal static void OnPlayersReady()
    {
        Resume();
        MultiplayerEvents.PlayersReady -= OnPlayersReady;
    }

    internal static bool TryResume(MultiplayerGame multiplayer)
    {
        if (!multiplayer.Players.Ready)
            return false;
        Resume();
        return true;
    }

    internal static void Resume()
    {
        if (SpeedControlScreen.Instance == null)
            return;
        if (SpeedControlScreen.Instance.IsPaused)
            SpeedControlScreen.Instance.Unpause();
    }
}
