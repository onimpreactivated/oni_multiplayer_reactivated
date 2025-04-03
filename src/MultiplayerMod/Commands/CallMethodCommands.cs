using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Wrappers;
using System.Reflection;

namespace MultiplayerMod.Commands;

internal class CallMethodCommands
{
    internal static void CallMethodCommand_Event(CallMethodCommand command)
    {
        var method = command.declaringType.GetMethod(
            command.methodName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
            BindingFlags.DeclaredOnly
        );
        var obj = command.target.Resolve();
        if (obj != null)
            method?.Invoke(obj, ArgumentUtils.UnWrapObjects(command.args));
    }
}
