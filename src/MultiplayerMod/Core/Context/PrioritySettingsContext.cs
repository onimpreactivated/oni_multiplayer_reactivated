namespace MultiplayerMod.Core.Context;

internal class PrioritySettingsContext(PrioritySetting priority) : IContext
{
    private PrioritySetting Priority => priority;

    public void Apply()
    {
        StaticContext.Override();
        if (StaticContext.Current.PriorityScreenInstance != null)
            StaticContext.Current.PriorityScreenInstance.lastSelectedPriority = Priority;
    }

    public void Restore()
    {
        StaticContext.Restore();
    }
}
