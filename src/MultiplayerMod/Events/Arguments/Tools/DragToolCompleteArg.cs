using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Events.Arguments.Tools;

public class DragToolCompleteArg(object sender, DragCompleteCommandArgs commandArg) : EventArgs
{
    public object Sender { get; } = sender;
    public DragCompleteCommandArgs CommandArg { get; } = commandArg;
}
