namespace MultiplayerMod.Extensions;

/// <summary>
/// 
/// </summary>
public static class ImmigrantScreenExtensions
{

    private const int DelayMS = 1;
    private const int MaxWaitMS = 50;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static async Task<List<ITelepadDeliverable>> WaitForAllDeliverablesReady(ImmigrantScreen instance)
    {
        var currentDelay = 0;
        while (currentDelay < MaxWaitMS)
        {
            var readyDeliverables = instance.containers?.Select(
                container => container switch
                {
                    CharacterContainer characterContainer => (ITelepadDeliverable) characterContainer.stats,
                    CarePackageContainer packageContainer => packageContainer.carePackageInstanceData,
                    _ => null
                }
            ).Where(deliverable => deliverable != null).ToList();
            if (readyDeliverables != null && readyDeliverables.Count == instance.containers?.Count)
                return readyDeliverables;

            await Task.Delay(DelayMS);
            currentDelay += DelayMS;
        }
        return null;
    }
}
