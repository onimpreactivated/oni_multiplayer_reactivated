namespace MultiplayerMod.Network.Common;

/// <summary>
/// Option for message command sending
/// </summary>
[Flags]
public enum MultiplayerCommandOptions : byte
{

    /// <summary>
    /// Default command behavior.
    /// </summary>
    None = 0,

    /// <summary>
    /// A command will not be sent to host client.
    /// </summary>
    SkipHost = 1,

    /// <summary>
    /// A command will not be sent to the current user.
    /// </summary>
    [Obsolete("This currently not have any functionality")]
    SkipSelf,

    /// <summary>
    /// A command will only be called on server.
    /// </summary>
    OnlyHost = 4,

}
