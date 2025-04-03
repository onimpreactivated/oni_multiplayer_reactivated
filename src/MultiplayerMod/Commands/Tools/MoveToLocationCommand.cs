using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// Command that let move into cell.
/// </summary>
/// <param name="navigator"></param>
/// <param name="movable"></param>
/// <param name="cell"></param>
[Serializable]
public class MoveToLocationCommand(Navigator navigator, Movable movable, int cell) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for <see cref="Navigator"/>
    /// </summary>
    public ComponentResolver<Navigator> NavigatorReference => navigator.GetComponentResolver();

    /// <summary>
    /// Resolver for <see cref="Movable"/>
    /// </summary>
    public ComponentResolver<Movable> MovableReference => movable.GetComponentResolver();

    /// <summary>
    /// Move to this cell
    /// </summary>
    public int Cell => cell;
}
