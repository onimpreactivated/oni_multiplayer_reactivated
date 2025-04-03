using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// Command that makes a building using <paramref name="args"/>
/// </summary>
/// <param name="args"></param>
[Serializable]
public class BuildCommand(BuildEventArgs args) : BaseCommandEvent
{
    /// <summary>
    /// Argument to build.
    /// </summary>
    public BuildEventArgs Args => args;
}
