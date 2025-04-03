using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Player;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Common;
using MultiplayerMod.Extensions;
using MultiplayerMod.Multiplayer.Datas.World;
using MultiplayerMod.Multiplayer.EventCalls;
using MultiplayerMod.Multiplayer.Interfaces;
using MultiplayerMod.Multiplayer.UI.Overlays;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.Multiplayer.Managers;

/// <summary>
/// World manager for Server
/// </summary>
/// <remarks>
/// Creates a new manager with <paramref name="stateManagers"/>
/// </remarks>
/// <param name="stateManagers"></param>
public class WorldManager(List<IWorldStateManager> stateManagers)
{
    private readonly List<IWorldStateManager> worldStateManagers = stateManagers;

    /// <summary>
    /// Sync the map between server and client.
    /// </summary>
    public void Sync()
    {

        SetupStatusOverlay();

        var resume = !SpeedControlScreen.Instance.IsPaused;
        MultiplayerManager.Instance.NetServer.Send(new PauseGameCommand());

        EventManager.TriggerEvent(new WorldSyncEvent());

        MultiplayerManager.Instance.MultiGame.Players.ForEach(it => MultiplayerManager.Instance.NetServer.Send(new ChangePlayerStateCommand(it.Id, PlayerState.Loading)));
        MultiplayerManager.Instance.NetServer.Send(new ChangePlayerStateCommand(MultiplayerManager.Instance.MultiGame.Players.Current.Id, PlayerState.Ready));
        MultiplayerManager.Instance.NetServer.Send(new NotifyWorldSavePreparingCommand(), MultiplayerCommandOptions.SkipHost);

        var world = new WorldSave(WorldName, GetWorldSave(), new WorldState());
        worldStateManagers.ForEach(it => it.SaveState(world.State));
        MultiplayerManager.Instance.NetServer.Send(new LoadWorldCommand(world), MultiplayerCommandOptions.SkipHost);
        EventManager.SubscribeEvent<PlayersReadyEvent>(MPServerCalls.ResumeGameOnReady);
    }

    private void SetupStatusOverlay()
    {
        MultiplayerStatusOverlay.Show("Waiting for players...");
        EventManager.SubscribeEvent<PlayerStateChangedEvent>(WaitPlayers);
    }

    /// <summary>
    /// Loading <paramref name="world"/> in the game.
    /// </summary>
    /// <param name="world"></param>
    public void RequestWorldLoad(WorldSave world)
    {
        MultiplayerStatusOverlay.Show($"Loading {world.Name}...");
        EventManager.SubscribeEvent<PlayersReadyEvent>(MPCommonEvents.CloseOverlayOnReady);
        EventManager.SubscribeEvent<WorldStateInitializingEvent>([UnsubAfterCall] (_) =>
        {
            worldStateManagers.ForEach(it => it.LoadState(world.State));
        });
        LoadWorldSave(world.Name, world.Data);
    }

    private void LoadWorldSave(string name, byte[] data)
    {
        var savePath = SaveLoader.GetCloudSavesDefault()
            ? SaveLoader.GetCloudSavePrefix()
            : SaveLoader.GetSavePrefixAndCreateFolder();

        var path = Path.Combine(savePath, name, $"{name}.sav");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, data);
        LoadScreen.DoLoad(path);
    }

    /// <summary>
    /// State changing that waits until all player ready then close Overlay
    /// </summary>
    /// <param name="event"></param>
    public void WaitPlayers(PlayerStateChangedEvent @event)
    {
        Debug.Log($"Player change: {@event.Player.Id} {@event.State}");
        var players = MultiplayerManager.Instance.MultiGame.Players;
        Debug.Log($"Players Ready: {players.Ready}");
        if (players.Ready)
        {
            MultiplayerStatusOverlay.Close();
            EventManager.UnsubscribeEvent<PlayerStateChangedEvent>(WaitPlayers);
        }
        var readyPlayersCount = players.Count(it => it.State == PlayerState.Ready);
        var playerList = string.Join("\n", players.Select(it => $"{it.Profile.PlayerName}: {it.State}"));
        var statusText = $"Waiting for players ({readyPlayersCount}/{players.Count} ready)...\n{playerList}";
        MultiplayerStatusOverlay.Text = statusText;
    }

    private static string WorldName => Path.GetFileNameWithoutExtension(SaveLoader.GetActiveSaveFilePath());

    private static byte[] GetWorldSave()
    {
        var path = SaveLoader.GetActiveSaveFilePath();
        SaveLoader.Instance.Save(path);
        return File.ReadAllBytes(path);
    }
}
