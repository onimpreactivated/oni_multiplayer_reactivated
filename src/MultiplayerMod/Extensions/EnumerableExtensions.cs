namespace MultiplayerMod.Extensions;

/// <summary>
/// 
/// </summary>
public static class EnumerableExtensions
{

    /// <summary>
    /// Extension method for using ForEach and calling <paramref name="action"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
            action(item);
    }
}
