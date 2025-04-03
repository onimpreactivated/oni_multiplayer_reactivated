namespace MultiplayerMod.Core.Exceptions;

/// <summary>
/// Exception for not finding <paramref name="stateMachine"/>
/// </summary>
/// <param name="stateMachine"></param>
/// <param name="name"></param>
public class StateMachineStateNotFoundException(StateMachine stateMachine, string name) : Exception($"State \"{name}\" not found in \"{stateMachine.name}\"");
