using MultiplayerMod.Extensions;

namespace MultiplayerMod.Core.Context;

internal class ContextArray(params IContext[] contexts) : IContext
{
    private IContext[] Contexts => contexts;

    public void Apply()
    {
        Contexts.ForEach(it => it.Apply());
    }

    public void Restore()
    {
        Contexts.ForEach(it => it.Restore());
    }
}
