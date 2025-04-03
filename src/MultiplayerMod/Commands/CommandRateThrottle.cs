using MultiplayerMod.Commands.NetCommands;

namespace MultiplayerMod.Commands;

/// <summary>
/// Create a Throttle for command sending. (Wait X time until send the last value.)
/// </summary>
/// <param name="rate"></param>
public class CommandRateThrottle(int rate)
{

    private readonly TimeSpan period = new(10000000 / rate);
    private readonly Dictionary<Type, System.DateTime> lastInvokedByType = [];

    /// <summary>
    /// Runs the <paramref name="action"/> if last invoked is smaller than <see cref="period"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    public void Run<T>(System.Action action) where T : BaseCommandEvent
    {
        lastInvokedByType.TryGetValue(typeof(T), out var lastInvoked);
        if (System.DateTime.Now - lastInvoked < period)
            return;
        action();
        lastInvokedByType[typeof(T)] = System.DateTime.Now;
    }
}
