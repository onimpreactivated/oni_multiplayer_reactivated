namespace MultiplayerMod.Core.Context;

/// <summary>
/// 
/// </summary>
[Serializable]
public class DebugToolContext : IContext
{
    /// <summary>
    /// <see cref="DebugPaintElementScreen.affectCells"/>
    /// </summary>
    public bool AffectCells { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.affectBuildings"/>
    /// </summary>
    public bool AffectBuildings { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.element"/>
    /// </summary>
    public SimHashes? Element { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.diseaseIdx"/>
    /// </summary>
    public byte? DiseaseType { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.diseaseCount"/>
    /// </summary>
    public int? DiseaseCount { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.temperature"/>
    /// </summary>
    public float? Temperature { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.mass"/>
    /// </summary>
    public float? Mass { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.set_prevent_fow_reveal"/>
    /// </summary>
    public bool PreventFowReveal { get; set; }

    /// <summary>
    /// <see cref="DebugPaintElementScreen.set_allow_fow_reveal"/>
    /// </summary>
    public bool AllowFowReveal { get; set; }

    /// <inheritdoc/>
    public void Apply()
    {
        StaticContext.Override();
        var instance = StaticContext.Current.DebugPaintElementScreenInstance;
        if (Element != null)
        {
            instance.paintElement.isOn = true;
            instance.element = Element.Value;
        }
        if (Temperature != null)
        {
            instance.paintTemperature.isOn = true;
            instance.temperature = Temperature.Value;
        }
        if (Mass != null)
        {
            instance.paintMass.isOn = true;
            instance.mass = Mass.Value;
        }
        if (DiseaseCount != null)
        {
            instance.paintDiseaseCount.isOn = true;
            instance.diseaseCount = DiseaseCount.Value;
        }
        if (DiseaseType != null)
        {
            instance.paintDisease.isOn = true;
            instance.diseaseIdx = DiseaseType.Value;
        }

        instance.affectBuildings.isOn = AffectBuildings;
        instance.affectCells.isOn = AffectCells;
        instance.set_prevent_fow_reveal = PreventFowReveal;
        instance.set_allow_fow_reveal = AllowFowReveal;
    }

    /// <inheritdoc/>
    public void Restore()
    {
        StaticContext.Restore();
    }
}
