using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// Command for <see cref="BaseUtilityBuildTool"/>
/// </summary>
/// <param name="args"></param>
/// <param name="senderType"></param>
[Serializable]
public class BuildUtilityCommand(UtilityBuildEventArgs args, Type senderType) : BaseCommandEvent
{
    /// <summary>
    /// Argument for <see cref="BaseUtilityBuildTool"/>
    /// </summary>
    public UtilityBuildEventArgs Args => args;

    /// <summary>
    /// Type of who sent this
    /// </summary>
    public Type SenderType => senderType;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{base.ToString()} Args: {Args}, SenderType: {SenderType}";
    }
}
