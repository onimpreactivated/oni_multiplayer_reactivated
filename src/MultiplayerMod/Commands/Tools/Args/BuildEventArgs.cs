namespace MultiplayerMod.Commands.Tools.Args;

/// <summary>
/// Event Argument for <see cref="BuildCommand"/>
/// </summary>
/// <param name="cell"></param>
/// <param name="prefabId"></param>
/// <param name="instantBuild"></param>
/// <param name="upgrade"></param>
/// <param name="orientation"></param>
/// <param name="materials"></param>
/// <param name="facadeId"></param>
/// <param name="priority"></param>
[Serializable]
public class BuildEventArgs(int cell, string prefabId, bool instantBuild, bool upgrade, Orientation orientation, Tag[] materials, string facadeId, PrioritySetting priority)
{
    /// <summary>
    /// The Cell to build
    /// </summary>
    public int Cell => cell;

    /// <summary>
    /// PrefabId for build
    /// </summary>
    public string PrefabId => prefabId;
    /// <summary>
    /// Should build instantly
    /// </summary>
    public bool InstantBuild => instantBuild;
    /// <summary>
    /// Should Upgrade
    /// </summary>
    public bool Upgrade => upgrade;
    /// <summary>
    /// Orientation to look
    /// </summary>
    public Orientation Orientation => orientation;
    /// <summary>
    /// Materails to build
    /// </summary>
    public Tag[] Materials => materials;
    /// <summary>
    /// The Facade Id
    /// </summary>
    public string FacadeId => facadeId;
    /// <summary>
    /// Priority to build
    /// </summary>
    public PrioritySetting Priority => priority;
}
