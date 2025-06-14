using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// State Machine related command that set the new state to <paramref name="statename"/> in <paramref name="resolver"/>
/// </summary>
/// <param name="resolver"></param>
/// <param name="statename"></param>
[Serializable]
public class GoToStateCommand : BaseCommandEvent
{
    /// <summary>
    /// State Machine related command that set the new state to <paramref name="state"/> in <paramref name="resolver"/>
    /// </summary>
    /// <param name="resolver"></param>
    /// <param name="state"></param>
    public GoToStateCommand(TypedResolver<StateMachine.Instance> resolver, StateMachine.BaseState state) :
        this(resolver, state?.name)
    { }

    public GoToStateCommand(TypedResolver<StateMachine.Instance> resolver, string statename)
    {
        Resolver = resolver;
        StateName = statename;
    }

    /// <summary>
    /// Resolver for <see cref="StateMachine.Instance"/>
    /// </summary>
    public TypedResolver<StateMachine.Instance> Resolver { get; }

    /// <summary>
    /// The new state it should go.
    /// </summary>
    public string StateName { get; }
}
