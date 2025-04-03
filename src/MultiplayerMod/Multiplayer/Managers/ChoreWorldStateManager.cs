using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Chores;
using MultiplayerMod.Extensions;
using MultiplayerMod.Multiplayer.Controllers;
using MultiplayerMod.Multiplayer.Datas.World;
using MultiplayerMod.Multiplayer.Interfaces;

namespace MultiplayerMod.Multiplayer.Managers;

/// <summary>
/// Sync Chore between clients
/// </summary>
public class ChoreWorldStateManager : IWorldStateManager
{
    internal Dictionary<Chore, ChoreCreatedEvent> CurrentChores = [];

    /// <summary>
    /// Sync Chore between clients
    /// </summary>
    public ChoreWorldStateManager()
    {
        EventManager.SubscribeEvent<ChoreCreatedEvent>(ChoreCreatedEvent_Call);
        EventManager.SubscribeEvent<ChoreCleanupEvent>(ChoreCleanupEvent_Call);
    }

    /// <inheritdoc/>
    public void LoadState(WorldState data)
    {
        var state = new ChoresState(data);

        foreach (var choreState in state.Chores)
        {
            var args = ChoreArgumentsWrapper.Unwrap(choreState.type, ArgumentUtils.UnWrapObjects(choreState.arguments));
            var chore = (Chore) choreState.type.GetConstructors()[0].Invoke(args);
            chore.Register(choreState.id);
        }

        foreach (var choreDriverState in state.Drivers)
        {
            var driver = choreDriverState.Driver.Resolve();
            var chore = choreDriverState.Chore.Resolve();
            var consumer = choreDriverState.Consumer.Resolve();
            var choreContext = new Chore.Precondition.Context(
                chore,
                new ChoreConsumerState(consumer),
                is_attempting_override: false
            );
            ChoresController.Set(driver, ref choreContext);
        }
    }

    /// <inheritdoc/>
    public void SaveState(WorldState data)
    {
        ChoresState state = new(data)
        {
            Chores = CurrentChores.Values.Where(x=>x.Id != null).Select(it => new ChoreState
            {
                id = it.Id,
                type = it.Type,
                arguments = ArgumentUtils.WrapObjects(ChoreArgumentsWrapper.Wrap(it.Type, it.Arguments))
            }).ToArray(),

            Drivers = UnityEngine.Object.FindObjectsOfType<ChoreDriver>()
            .Where(it =>
            {
                var chore = it.GetCurrentChore();
                if (chore == null)
                    return false;

                var multiplayerObject = MultiplayerManager.Instance.MPObjects.Get(chore);
                return multiplayerObject != null && multiplayerObject.Persistent;
            })
            .Select(it => new ChoreDriverState
            {
                Driver = it.GetComponentResolver(),
                Consumer = it.context.consumerState.consumer.GetComponentResolver(),
                Chore = new ChoreResolver(it.GetCurrentChore())
            })
            .ToArray()
        };
    }

    internal void ChoreCreatedEvent_Call(ChoreCreatedEvent @event)
    {
        CurrentChores.Add(@event.Chore, @event);
    }

    internal void ChoreCleanupEvent_Call(ChoreCleanupEvent @event)
    {
        if (@event.Chore == null)
            return;
        CurrentChores.Remove(@event.Chore);
    }

    private class ChoresState(WorldState state)
    {
        private const string choresKey = "chores";
        private const string choreDriversKey = "drivers";

        public ChoreState[] Chores
        {
            get => (ChoreState[]) state.Entries[choresKey];
            set => state.Entries[choresKey] = value;
        }

        public ChoreDriverState[] Drivers
        {
            get => (ChoreDriverState[]) state.Entries[choreDriversKey];
            set => state.Entries[choreDriversKey] = value;
        }
    }

    [Serializable]
    private struct ChoreState
    {
        public MultiplayerId id;
        public Type type;
        public object[] arguments;
    }

    [Serializable]
    private struct ChoreDriverState
    {
        public ComponentResolver<ChoreDriver> Driver;
        public ComponentResolver<ChoreConsumer> Consumer;
        public ChoreResolver Chore;
    }
}
