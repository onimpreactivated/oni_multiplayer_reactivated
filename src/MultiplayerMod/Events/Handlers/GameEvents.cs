using MultiplayerMod.Events.Arguments.Common;
using UnityEngine;

namespace MultiplayerMod.Events.Handlers;

public static class GameEvents
{
    public static event OniEventHandler GameQuit;
    public static event OniEventHandler GameReady;
    public static event OniEventHandler GameStarted;
    public static event OniEventHandlerTEventArgs<GameObjectArg> GameObjectCreated;

    public static void OnGameQuit()
    {
        GameQuit?.Invoke();
    }

    public static void OnGameStarted()
    {
        GameStarted?.Invoke();
    }

    public static void OnGameReady()
    {
        GameReady?.Invoke();
    }

    public static void OnGameObjectCreated(GameObject @object)
    {
        GameObjectCreated?.Invoke(new(@object));
    }
}
