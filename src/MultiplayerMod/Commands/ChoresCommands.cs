using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core.Weak;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;
using MultiplayerMod.Multiplayer.Controllers;

namespace MultiplayerMod.Commands;

internal static class ChoresCommands
{
    internal static void CreateChoreCommand_Event(CreateChoreCommand command)
    {
        var args = ChoreArgumentsWrapper.Unwrap(command.ChoreType, ArgumentUtils.UnWrapObjects(command.Arguments));
        Debug.Log($"Create chore {command.ChoreType} [id={command.MultiId}]");
        var chore = (Chore) command.ChoreType.GetConstructors()[0].Invoke(args);
        chore.Register(command.MultiId);
    }

    internal static void ReleaseChoreDriverCommand_Event(ReleaseChoreDriverCommand command)
    {
        var driver = command.DriverReference.Resolve();
        ChoresController.Release(driver);
    }

    internal static void SetDriverChoreCommand_Event(SetDriverChoreCommand command)
    {
        var chore = command.ChoreReference.Resolve();
        var driver = command.DriverReference.Resolve();
        Chore.Precondition.Context choreContext;

        // TODO: A temporary solution until all chores are synced.
        // TODO: Now there can be a case when a consumer doesn't have required components.
        try
        {
            choreContext = new Chore.Precondition.Context(
                chore,
                new ChoreConsumerState(command.ConsumerReference.Resolve()),
                is_attempting_override: false,
                ArgumentUtils.UnWrapObject(command.Data)
            );
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Unable to create chore context:\n{exception.StackTrace}");
            return;
        }
        ChoresController.Set(driver, ref choreContext);
    }

    internal static void GoToStateCommand_Event(GoToStateCommand command)
    {
        var state_instance = command.Resolver.Resolve();
        StateMachineWeak.Get(state_instance).GoToState(command.StateName);
    }

    internal static void MoveObjectToCellCommand_Event(MoveObjectToCellCommand command)
    {
        var weak = StateMachineWeak.Get(command.Reference.Resolve());
        weak.FindParameter(MoveObjectToCellCommand.TargetCell)?.Set(command.Cell);
        weak.GoToState(command.MovingStateName);
    }

    internal static void SynchronizeObjectPositionCommand_Event(SynchronizeObjectPositionCommand command)
    {
        var gameObject = command.Resolver.Resolve();
        gameObject.transform.SetPosition(command.Position);
        if (command.FacingLeft != null)
            gameObject.GetComponent<Facing>().SetFacing(command.FacingLeft.Value);
    }

    internal static void SetParameterValueCommand_Event(SetParameterValueCommand command)
    {
        var instance = command.Controller.Resolve().GetSMI(command.StateMachineInstanceType);
        var parameterContext = instance.parameterContexts[command.ParameterIndex];
        var parameterValue = ArgumentUtils.UnWrapObject(command.Value);
        StateMachineContextWeak.Get(parameterContext).Set(instance, parameterValue);
    }
}
