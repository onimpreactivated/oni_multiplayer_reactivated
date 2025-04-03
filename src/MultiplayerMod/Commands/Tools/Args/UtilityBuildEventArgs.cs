namespace MultiplayerMod.Commands.Tools.Args;

/// <summary>
/// Arguments for <see cref="UtilityBuildTool"/>
/// </summary>
/// <param name="prefabId"></param>
/// <param name="materials"></param>
/// <param name="path"></param>
/// <param name="priority"></param>
[Serializable]
public class UtilityBuildEventArgs(string prefabId, Tag[] materials, List<BaseUtilityBuildTool.PathNode> path, PrioritySetting priority)
{
    /// <summary>
    /// The prefab Id
    /// </summary>
    public string PrefabId => prefabId;

    /// <summary>
    /// Materials to build
    /// </summary>
    public Tag[] Materials => materials;

    /// <summary>
    /// Path that build to
    /// </summary>
    public List<BaseUtilityBuildTool.PathNode> Path => path;

    /// <summary>
    /// Priority for the build
    /// </summary>
    public PrioritySetting Priority => priority;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"PrefabId: {PrefabId}, Materials: {string.Join(", ", Materials)}, Path: {string.Join(", ", Path)}, Priority: {Priority}";
    }
}
