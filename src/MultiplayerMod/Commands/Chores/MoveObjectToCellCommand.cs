using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Called when Chore must move to a different cell.
/// </summary>
/// <param name="reference"></param>
/// <param name="cell"></param>
/// <param name="movingStateName"></param>
[Serializable]
public class MoveObjectToCellCommand(TypedResolver<StateMachine.Instance> reference, int cell, string movingStateName) : BaseCommandEvent
{
    /// <summary>
    /// <see cref="ParameterInfo{T}"/> for movint to target cell.
    /// </summary>
    public static ParameterInfo<int> TargetCell = new(
        "__move_to_target_cell",
        defaultValue: Grid.InvalidCell
    );

    /// <summary>
    /// Resolver for <see cref="StateMachine.Instance"/>
    /// </summary>
    public TypedResolver<StateMachine.Instance> Reference => reference;

    /// <summary>
    /// Name of the moving state
    /// </summary>
    public string MovingStateName => movingStateName;

    /// <summary>
    /// Cell to move to
    /// </summary>
    public int Cell => cell;

    /// <summary>
    /// Called when Chore must move to a different cell.
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="cell"></param>
    /// <param name="movingState"></param>
    public MoveObjectToCellCommand(TypedResolver<StateMachine.Instance> reference, int cell, StateMachine.BaseState movingState) :
        this(reference, cell, movingState?.name)
    { }

    /// <summary>
    /// Called when Chore must move to a different cell.
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="cell"></param>
    /// <param name="movingStateInfo"></param>
    public MoveObjectToCellCommand(TypedResolver<StateMachine.Instance> reference,int cell, StateInfo movingStateInfo) :
        this(reference, cell, movingStateInfo?.ReferenceName)
    { }
}
