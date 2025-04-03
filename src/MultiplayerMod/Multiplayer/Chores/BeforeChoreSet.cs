using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Events.Chores;
using MultiplayerMod.Extensions;
using MultiplayerMod.Multiplayer.Controllers;
using MultiplayerMod.Network.Common;
using System;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Multiplayer.Chores;

internal class BeforeChoreSet
{
    private static readonly ConditionalWeakTable<ChoreDriver, BoxedValue<bool>> driverSynchronizationState = new();

    internal static void BeforeChoreSetEvent_Call(BeforeChoreSetEvent @event)
    {
        if (!MultiplayerManager.IsMultiplayer())
            return;

        var synchronized = driverSynchronizationState.GetValue(@event.Driver, _ => new BoxedValue<bool>(false));
        var shouldReleaseDriver = synchronized.Value && @event.PreviousChore != null && ChoresController.Supported(@event.PreviousChore);
        if (shouldReleaseDriver)
        {
            MultiplayerManager.Instance.NetServer.Send(new ReleaseChoreDriverCommand(@event.Driver), MultiplayerCommandOptions.SkipHost);
            synchronized.Value = false;
        }

        if (!ChoresController.Supported(@event.Context.chore))
            return;

        if (!@event.Context.chore.IsValid_Ext())
            return;
        if (!MultiplayerManager.Instance.MultiGame.Players.Ready)
            return;
        MultiplayerManager.Instance.NetServer.Send(
            new SetDriverChoreCommand(@event.Driver, @event.Context.consumerState.consumer, @event.Context.chore, @event.Context.data),
            MultiplayerCommandOptions.SkipHost);
        synchronized.Value = true;
    }
}
