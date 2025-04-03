namespace MultiplayerMod.Core.Wrappers;

/// <summary>
/// This is a stub class for referencing
/// <see cref="GameStateMachine{StateMachineType,StateMachineInstanceType,MasterType,DefType}"/>
/// members via nameof.
/// </summary>
public class StateMachineMemberReference : GameStateMachine<StateMachineMemberReference, StateMachineMemberReference.Instance, KMonoBehaviour, object>
{
    /// <inheritdoc/>
    public new class Instance : GameInstance
    {
        /// <inheritdoc/>
        public Instance(KMonoBehaviour master, object def) : base(master, def) { }

        /// <inheritdoc/>
        public Instance(KMonoBehaviour master) : base(master) { }
    }

    /// <inheritdoc/>
    public new class Parameter : Parameter<object>
    {
        /// <inheritdoc/>
        public override StateMachine.Parameter.Context CreateContext() => throw new NotImplementedException();

        /// <inheritdoc/>
        public new class Context : Parameter<object>.Context
        {
            /// <inheritdoc/>
            public Context(StateMachine.Parameter parameter, object default_value) : base(parameter, default_value) { }

            /// <inheritdoc/>
            public override void Serialize(BinaryWriter writer)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc/>
            public override void Deserialize(IReader reader, StateMachine.Instance smi)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc/>
            public override void ShowEditor(StateMachine.Instance base_smi)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc/>
            public override void ShowDevTool(StateMachine.Instance base_smi)
            {
                throw new NotImplementedException();
            }
        }
    }

}

