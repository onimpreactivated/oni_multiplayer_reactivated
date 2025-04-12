using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Tools;


[Serializable]
public class MoveToLocationCommand : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="Navigator"/>
    /// </summary>
    public ComponentResolver<Navigator> NavigatorReference { get; }

    /// <summary>
    /// Resolver for <see cref="Movable"/>
    /// </summary>
    public ComponentResolver<Movable> MovableReference { get; }

    /// <summary>
    /// Move to this cell
    /// </summary>
    public int Cell { get; }

    /// <summary>
    /// Command that let move into cell.
    /// </summary>
    /// <param name="navigator"></param>
    /// <param name="movable"></param>
    /// <param name="cell"></param>
    public MoveToLocationCommand(Navigator navigator, Movable movable, int cell)
    {
        NavigatorReference = navigator.GetComponentResolver();
        MovableReference = movable.GetComponentResolver();
        Cell = cell;
    }
}
