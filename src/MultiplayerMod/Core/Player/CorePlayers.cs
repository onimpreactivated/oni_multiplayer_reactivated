using MultiplayerMod.Extensions;
using System.Collections;

namespace MultiplayerMod.Core.Player;

/// <summary>
/// Advanced <see cref="IEnumerable{T}"/> for <see cref="CorePlayer"/>
/// </summary>
public class CorePlayers : IEnumerable<CorePlayer>
{
    private readonly Dictionary<Guid, CorePlayer> Players = [];

    private Guid currentPlayerId = Guid.Empty;

    /// <summary>
    /// Add new Player
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public CorePlayer Add(CorePlayer player) => Players[player.Id] = player;

    /// <summary>
    /// Remove <see cref="CorePlayer"/> with <paramref name="id"/>
    /// </summary>
    /// <param name="id">Identification of a <see cref="CorePlayer"/></param>
    /// <returns>Success or Failure</returns>

    public bool Remove(Guid id) => Players.Remove(id);

    /// <summary>
    /// Getting 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CorePlayer this[Guid id] => Players[id];

    /// <summary>
    /// Checks if all <see cref="Players"/> are <see cref="PlayerState.Ready"/>
    /// </summary>
    public bool Ready => Players.Values.All(player => player.State == PlayerState.Ready);

    /// <summary>
    /// Getting the <see cref="CorePlayer"/> information as <see cref="IEnumerator{T}"/>
    /// </summary>
    /// <returns></returns>
    public IEnumerator<CorePlayer> GetEnumerator() => Players.Values.GetEnumerator();

    /// <summary>
    /// Number of <see cref="CorePlayer"/> are in <see cref="Players"/>
    /// </summary>
    public int Count => Players.Values.Count;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// The Current <see cref="CorePlayer"/> accorind to <see cref="currentPlayerId"/>
    /// </summary>
    public CorePlayer Current => Players[currentPlayerId];

    /// <summary>
    /// Syncronize <see cref="CorePlayers"/> between <see cref="PlayerRole.Server"/> and <see cref="PlayerRole.Client"/>
    /// </summary>
    /// <param name="players"></param>
    public void Synchronize(IEnumerable<CorePlayer> players)
    {
        Players.Clear();
        players.ForEach(it => Players[it.Id] = it);
    }

    /// <summary>
    /// Setting the <see cref="currentPlayerId"/> internally as <paramref name="playerId"/>
    /// </summary>
    /// <param name="playerId"></param>
    public void SetCurrentPlayerId(Guid playerId)
    {
        currentPlayerId = playerId;
    }
}
