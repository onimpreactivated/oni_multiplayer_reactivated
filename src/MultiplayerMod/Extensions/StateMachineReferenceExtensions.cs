using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Extensions;

/// <summary>
/// Extension for <see cref="StateMachine.Instance"/>
/// </summary>
public static class StateMachineReferenceExtensions
{
    /// <summary>
    /// Getting <see cref="StateMachineResolver"/> from <paramref name="instance"/>
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static StateMachineResolver GetSMResolver(this StateMachine.Instance instance) => new(
        instance.GetStateMachineController().GetComponentResolver(),
        instance.GetType()
    );

    // `controller` field is defined in StateMachine<,,,>.GenericInstance. However cast is impossible due to unknown
    // generic argument types. So reflection is the most handy way to get its value :(
    private static StateMachineController GetStateMachineController(this StateMachine.Instance instance) =>
        instance.GetFieldValue<StateMachineController>("controller");
}
