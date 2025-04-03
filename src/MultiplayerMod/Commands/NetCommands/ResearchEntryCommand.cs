namespace MultiplayerMod.Commands.NetCommands;

/// <summary>
/// Simple command that let cancel or start the research.
/// </summary>
/// <param name="techId"></param>
/// <param name="isCancel"></param>
[Serializable]
public class ResearchEntryCommand(string techId, bool isCancel) : BaseCommandEvent
{
    /// <summary>
    /// Tech Id to either start researching or cancel its research
    /// </summary>
    public string TechId => techId;

    /// <summary>
    /// Should the research for <see cref="TechId"/> be cancelled or not
    /// </summary>
    public bool IsCancel => isCancel;
}
