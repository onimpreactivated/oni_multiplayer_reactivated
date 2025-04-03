namespace MultiplayerMod.Events.Arguments.Common;

public class IntArg(int value) : EventArgs
{
    public int Value { get; } = value;
}
