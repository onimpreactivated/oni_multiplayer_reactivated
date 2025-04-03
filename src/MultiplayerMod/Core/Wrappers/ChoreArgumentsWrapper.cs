using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;
using STRINGS;

namespace MultiplayerMod.Core.Wrappers;

/// <summary>
/// Argument wrapper for <see cref="Chore"/>
/// </summary>
public static class ChoreArgumentsWrapper
{
    /// <summary>
    /// Wrap the <paramref name="choreType"/> and <paramref name="args"/> to able to send with network.
    /// </summary>
    /// <param name="choreType"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static object[] Wrap(Type choreType, object[] args)
    {
        if (choreType == typeof(MoveChore))
        {
            args[2] = ((Func<MoveChore.StatesInstance, int>) args[2]!).Invoke(null!);
        }
        if (choreType == typeof(VomitChore))
        {
            args[2] = null;
            args[3] = null;
        }
        if (choreType == typeof(BansheeChore))
        {
            args[2] = null;
        }
        if (choreType == typeof(FetchAreaChore))
        {
            var context = (Chore.Precondition.Context) args[0]!;
            if (!context.chore.IsValid_Ext())
                context.chore.Register();
            return
            [
                context.chore.MultiplayerId(),
                context.consumerState.consumer.GetComponentResolver(),
                context.consumerState.choreProvider.GetComponentResolver(),
                context.masterPriority.priority_class,
                context.masterPriority.priority_value
            ];
        }
        return args;
    }

    /// <summary>
    /// UnWrap the <paramref name="choreType"/> and <paramref name="args"/> from the received network.
    /// </summary>
    /// <param name="choreType"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static object[] Unwrap(Type choreType, object[] args)
    {
        Debug.Log(choreType);
        foreach (object arg in args)
        {
            Debug.Log(arg);
            Debug.Log(arg.GetType());
        }

        if (choreType == typeof(MoveChore))
        {
            var targetCell = (int) args[2]!;
            args[2] = new Func<MoveChore.StatesInstance, int>(_ => targetCell);
        }
        if (choreType == typeof(VomitChore))
        {
            args[2] = Db.Get().DuplicantStatusItems.Vomiting;
            args[3] = new Notification(
                (string) DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_NAME,
                NotificationType.Bad,
                (notificationList, data) =>
                    (string) DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_TOOLTIP +
                    notificationList.ReduceMessages(false)
            );
        }
        if (choreType == typeof(BansheeChore))
        {
            args[2] = new Notification(
                (string) DUPLICANTS.MODIFIERS.BANSHEE_WAILING.NOTIFICATION_NAME,
                NotificationType.Bad,
                (notificationList, data) =>
                    (string) DUPLICANTS.MODIFIERS.BANSHEE_WAILING.NOTIFICATION_TOOLTIP +
                    notificationList.ReduceMessages(false)
            );
        }
        if (choreType == typeof(FetchAreaChore))
        {
            var choreId = (MultiplayerId) args[0]!;
            var choreConsumer = (ComponentResolver<ChoreConsumer>) args[1]!;
            var choreProvider = (ComponentResolver<ChoreProvider>) args[2]!;
            var priorityClass = (PriorityScreen.PriorityClass) args[3]!;
            var priorityValue = (int) args[4]!;
            args = [
                new Chore.Precondition.Context {
                    chore = MultiplayerManager.Instance.MultiGame.Objects.Get<Chore>(choreId),
                    consumerState = new ChoreConsumerState(choreConsumer.Resolve()) {
                        choreProvider = choreProvider.Resolve()
                    },
                    masterPriority = new PrioritySetting(priorityClass, priorityValue)
                }
            ];
        }
        return args;
    }
}
