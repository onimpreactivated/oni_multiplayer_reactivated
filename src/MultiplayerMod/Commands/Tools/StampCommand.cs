using MultiplayerMod.Commands.NetCommands;
using UnityEngine;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// Stamp command
/// </summary>
/// <param name="template"></param>
/// <param name="location"></param>
[Serializable]
public class StampCommand(
    TemplateContainer template,
    Vector2 location) : BaseCommandEvent
{
    /// <summary>
    /// Stamp Template
    /// </summary>
    public TemplateContainer Template => template;

    /// <summary>
    /// Stamp Location
    /// </summary>
    public Vector2 Location => location;
}
