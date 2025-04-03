namespace MultiplayerMod.Events.Arguments.Common;

public class IntArg : EventArgs
{
    public int Value { get; }

    public IntArg(int value)
    {
        Value = value;
    }
}
