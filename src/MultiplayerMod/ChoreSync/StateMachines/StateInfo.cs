namespace MultiplayerMod.ChoreSync.StateMachines;

/// <summary>
/// State info for StateMachine.
/// </summary>
/// <param name="name"></param>
public class StateInfo(string name)
{
    /// <summary>
    /// Name of the State
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Reference name of <see cref="Name"/> | "root.{name}"
    /// </summary>
    public string ReferenceName { get; } = $"root.{name}";
}
