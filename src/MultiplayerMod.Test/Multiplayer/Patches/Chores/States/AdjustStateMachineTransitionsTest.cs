using System;
using System.Collections.Generic;
using System.Linq;
using MultiplayerMod.Core.Dependency;
using MultiplayerMod.Game.Chores.Types;
using MultiplayerMod.ModRuntime;
using MultiplayerMod.Multiplayer;
using MultiplayerMod.Multiplayer.Patches.Chores.States;
using MultiplayerMod.Multiplayer.States;
using MultiplayerMod.Test.Game.Chores;
using NUnit.Framework;

namespace MultiplayerMod.Test.Multiplayer.Patches.Chores.States;

[TestFixture]
public class DisableChoreStateTransitionTest : AbstractChoreTest {

    [OneTimeSetUp]
    public static void OneTimeSetUp() {
        Runtime.Instance.Dependencies.Get<MultiplayerGame>().Refresh(MultiplayerMode.Client);

        var di = (DependencyContainer) Runtime.Instance.Dependencies;
        di.Register(new DependencyInfo(nameof(FakeStatesManager), typeof(FakeStatesManager), false));
    }

    [SetUp]
    public void SetUp() {
        CreateTestData(new HashSet<Type> { typeof(AdjustStateMachineTransitions) });
        Singleton<StateMachineManager>.Instance.Clear();
    }

    [Test, TestCaseSource(nameof(EnterTestArgs))]
    public void ClientStateEnter_DisablesTransition(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        var statesManager = Runtime.Instance.Dependencies.Get<FakeStatesManager>();
        statesManager.WaitArgs.Clear();
        statesManager.ContinuationArgs.Clear();

        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var smi = statesManager.GetSmi(chore);
        var expectedState = config.GetMonitoredState(smi.GetStateMachine());
        Assert.Contains(expectedState, statesManager.WaitArgs);
        Assert.Contains(expectedState, statesManager.ContinuationArgs);
    }

    [Test, TestCaseSource(nameof(ExitTestArgs))]
    public void ClientStateExit_DisablesTransition(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        var statesManager = Runtime.Instance.Dependencies.Get<FakeStatesManager>();
        statesManager.WaitArgs.Clear();

        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var smi = statesManager.GetSmi(chore);
        var expectedState = config.GetMonitoredState(smi.GetStateMachine());
        Assert.Contains(expectedState, statesManager.WaitArgs);
    }

    [Test, TestCaseSource(nameof(UpdateTestArgs))]
    public void ClientMustDisableUpdateCalls(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        Runtime.Instance.Dependencies.Get<MultiplayerGame>().Refresh(MultiplayerMode.Client);
        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var statesManager = Runtime.Instance.Dependencies.Get<StatesManager>();
        var smi = statesManager.GetSmi(chore);
        Assert.IsEmpty(config.GetMonitoredState(smi.stateMachine).updateActions);
    }

    [Test, TestCaseSource(nameof(UpdateTestArgs))]
    public void HostMustKeepUpdateCalls(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        Runtime.Instance.Dependencies.Get<MultiplayerGame>().Refresh(MultiplayerMode.Host);
        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var statesManager = Runtime.Instance.Dependencies.Get<StatesManager>();
        var smi = statesManager.GetSmi(chore);
        Assert.IsNotEmpty(config.GetMonitoredState(smi.stateMachine).updateActions);
    }

    [Test, TestCaseSource(nameof(EventHandlerTestArgs))]
    public void ClientMustDisableEventCalls(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        Runtime.Instance.Dependencies.Get<MultiplayerGame>().Refresh(MultiplayerMode.Client);
        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var statesManager = Runtime.Instance.Dependencies.Get<StatesManager>();
        var smi = statesManager.GetSmi(chore);
        Assert.IsEmpty(config.GetMonitoredState(smi.stateMachine).events);
    }

    [Test, TestCaseSource(nameof(EventHandlerTestArgs))]
    public void HostMustKeepEventCalls(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        Runtime.Instance.Dependencies.Get<MultiplayerGame>().Refresh(MultiplayerMode.Host);
        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var statesManager = Runtime.Instance.Dependencies.Get<StatesManager>();
        var smi = statesManager.GetSmi(chore);
        Assert.IsNotEmpty(config.GetMonitoredState(smi.stateMachine).events);
    }

    [Test, TestCaseSource(nameof(TransitionTestArgs))]
    public void ClientMustDisableTransitionCalls(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        Runtime.Instance.Dependencies.Get<MultiplayerGame>().Refresh(MultiplayerMode.Client);
        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var statesManager = Runtime.Instance.Dependencies.Get<StatesManager>();
        var smi = statesManager.GetSmi(chore);
        var state = config.GetMonitoredState(smi.stateMachine);
        Assert.False(state.enterActions.Any(it => it.name.Contains("Transition")));
        Assert.False(state.updateActions.Any(it => it.buckets.Any(bucket => bucket.name.Contains("Transition"))));
    }

    [Test, TestCaseSource(nameof(TransitionTestArgs))]
    public void HostMustKeepTransitionCalls(
        Type choreType,
        Func<object[]> choreArgsFunc,
        Func<Dictionary<int, object?>> stateTransitionArgsFunc,
        StateTransitionConfig config
    ) {
        Runtime.Instance.Dependencies.Get<MultiplayerGame>().Refresh(MultiplayerMode.Host);
        var chore = CreateChore(choreType, choreArgsFunc.Invoke());

        var statesManager = Runtime.Instance.Dependencies.Get<StatesManager>();
        var smi = statesManager.GetSmi(chore);
        Assert.IsNotEmpty(config.GetMonitoredState(smi.stateMachine).enterActions);
        Assert.IsNotEmpty(config.GetMonitoredState(smi.stateMachine).updateActions);
    }

    private class FakeStatesManager : StatesManager {
        public readonly List<StateMachine.BaseState> WaitArgs = new();
        public readonly List<StateMachine.BaseState> ContinuationArgs = new();

        public override void AddAndTransitToWaiStateUponEnter(StateMachine.BaseState stateToBeSynced) {
            WaitArgs.Add(stateToBeSynced);
        }

        public override StateMachine.BaseState AddContinuationState(StateMachine.BaseState stateToBeSynced) {
            ContinuationArgs.Add(stateToBeSynced);
            return null!;
        }
    }

}
