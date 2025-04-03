namespace MultiplayerMod.Core.Context;

internal static class ContextRunner
{
    public static void Override(IContext context, System.Action action)
    {
        try
        {
            context.Apply();
            action();
        }
        finally
        {
            context.Restore();
        }
    }
}
