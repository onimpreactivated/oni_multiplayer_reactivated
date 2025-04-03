using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Extensions;

/// <summary>
/// Extensions for type <see cref="Chore"/>
/// </summary>
public static class ChoreExtensions
{
    /// <summary>
    /// Register current chore object to <see cref="MultiplayerObjects"/>
    /// </summary>
    /// <param name="chore"></param>
    /// <param name="multiplayerId"></param>
    /// <param name="persistent"></param>
    /// <returns></returns>
    public static MultiplayerId Register(
        this Chore chore,
        MultiplayerId multiplayerId = null,
        bool persistent = false
    ) => MultiplayerManager.Instance.MPObjects.Register(chore, multiplayerId, persistent).Id;

    /// <summary>
    /// Getting the <see cref="MultiplayerId"/> of <paramref name="chore"/>
    /// </summary>
    /// <param name="chore"></param>
    /// <returns></returns>
    public static MultiplayerId MultiplayerId(this Chore chore) => MultiplayerManager.Instance.MPObjects.Get(chore)!.Id;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chore"></param>
    /// <returns></returns>
    public static bool IsValid_Ext(this Chore chore) => MultiplayerManager.Instance.MPObjects.Get(chore) != null;

    /// <summary>
    /// Get the Type Resolver for this type of Chore
    /// </summary>
    /// <param name="chore"></param>
    /// <returns></returns>
    public static ChoreResolver GetResolver(this Chore chore) => new(chore);

    /// <summary>
    /// Getting the <see cref="StateMachine.Instance"/> from the <see cref="Chore"/>
    /// </summary>
    /// <param name="chore"></param>
    /// <returns></returns>
    public static StateMachine.Instance GetSMI_Ext(this Chore chore)
    {
        return (StateMachine.Instance) chore.GetType().GetProperty(nameof(Chore<StateMachine.Instance>.smi)).GetValue(chore);
    }

}
