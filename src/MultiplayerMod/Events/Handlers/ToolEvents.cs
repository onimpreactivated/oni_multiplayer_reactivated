using MultiplayerMod.Events.Arguments.Tools;

namespace MultiplayerMod.Events.Handlers;

public static class ToolEvents
{

    public static event OniEventHandlerTEventArgs<DragToolCompleteArg> DragToolComplete;

    public static void OnDragToolComplete(DragToolCompleteArg args)
    {
        DragToolComplete?.Invoke(args);
    }
}
