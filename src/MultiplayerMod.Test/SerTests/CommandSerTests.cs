using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Serialization;
using NUnit.Framework;
using System;

namespace MultiplayerMod.Test.SerTests;

internal class CommandSerTests
{
    [Test]
    public void Test_UpdatePlayerCursorPositionCommand()
    {
        UpdatePlayerCursorPositionCommand command = new(
            Guid.NewGuid(),
            new Events.EventArgs.MouseMovedEventArgs(UnityEngine.Vector3.one, UnityEngine.Vector3.one, "test", "test")
        );
        var x = CoreSerializer.Serialize(command);
        Assert.IsNotEmpty(x);
    }

    [Test]
    public void Test_SetDefaultPriorityCommand()
    {
        SetDefaultPriorityCommand command = new("id", 5);
        var x = CoreSerializer.Serialize(command);
        Assert.IsNotEmpty(x);
    }
}
