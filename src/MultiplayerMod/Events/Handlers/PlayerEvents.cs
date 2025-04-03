using MultiplayerMod.Core.Player;
using MultiplayerMod.Events.Arguments.CorePlayerArgs;

namespace MultiplayerMod.Events.Handlers;

public static class PlayerEvents
{
    public static event OniEventHandlerTEventArgs<CorePlayerArg> CurrentPlayerInitialized;
    public static event OniEventHandlerTEventArgs<CorePlayerArg> PlayerJoined;
    public static event OniEventHandlerTEventArgs<CorePlayerLeftArg> PlayerLeft;
    public static event OniEventHandlerTEventArgs<CorePlayerStateChanged> PlayerStateChanged;
    public static event OniEventHandlerTEventArgs<CorePlayersUpdatedArgs> PlayersUpdated;

    public static void OnCurrentPlayerInitialized(CorePlayer player)
    {
        CurrentPlayerInitialized?.Invoke(new(player));
    }

    public static void OnPlayerJoined(CorePlayer player)
    {
        PlayerJoined?.Invoke(new(player));
    }

    public static void OnPlayerLeft(CorePlayer player, bool IsForced)
    {
        PlayerLeft?.Invoke(new(player, IsForced));
    }

    public static void OnPlayerStateChanged(CorePlayer player, PlayerState state)
    {
        PlayerStateChanged?.Invoke(new(player, state));
    }

    public static void OnPlayersUpdated(CorePlayers players)
    {
        PlayersUpdated?.Invoke(new(players));
    }
}
