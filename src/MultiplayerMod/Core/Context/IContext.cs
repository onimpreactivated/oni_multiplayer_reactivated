namespace MultiplayerMod.Core.Context;

/// <summary>
/// Contect for loading and apply fake instance
/// </summary>
public interface IContext
{
    /// <summary>
    /// Apply fake instance
    /// </summary>
    public void Apply();

    /// <summary>
    /// restore real instance
    /// </summary>
    public void Restore();
}
