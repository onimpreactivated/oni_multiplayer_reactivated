using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// Modify with <see cref="DebugTool"/>
/// </summary>
/// <param name="args"></param>
[Serializable]
public class ModifyCommand(ModifyEventArgs args) : BaseCommandEvent
{
    /// <summary>
    /// <see cref="ModifyEventArgs"/>
    /// </summary>
    public ModifyEventArgs Args => args;
}
