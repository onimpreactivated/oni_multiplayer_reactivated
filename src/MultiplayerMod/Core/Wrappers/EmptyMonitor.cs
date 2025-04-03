namespace MultiplayerMod.Core.Wrappers;

/// <summary>
/// Empty Monitor GameState Machine
/// </summary>
public class EmptyMonitor : GameStateMachine<EmptyMonitor, EmptyMonitor.Instance>
{
    /// <inheritdoc/>
    public new class Instance : GameInstance
    {
        /// <inheritdoc/>
        public Instance(IStateMachineTarget master)
            : base(master)
        {
        }
    }

    /// <summary>
    /// Fake state that does nothing.
    /// </summary>
    public State no_state;

    /// <inheritdoc/>
    public override void InitializeStates(out BaseState default_state)
    {
        default_state = no_state;
        no_state.DoNothing();
    }

    /// <summary>
    /// Creating <see cref="EmptyChore"/>
    /// </summary>
    /// <param name="smi"></param>
    /// <returns></returns>
    public Chore CreateEmptyChore(Instance smi)
    {
        return new EmptyChore(smi.master);
    }
}

