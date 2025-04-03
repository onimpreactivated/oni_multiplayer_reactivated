using MultiplayerMod.Core.Unity;
using UnityEngine.UI;

namespace MultiplayerMod.Core.Context;

/// <summary>
/// Static context for helping Multiplayer Commands to call clients Menu functions
/// </summary>
public class StaticContext
{
    /// <summary>
    /// Public <see cref="ToolMenu"/> Instance
    /// </summary>
    public ToolMenu ToolMenuInstance { get; }

    /// <summary>
    /// Public <see cref="BuildMenu"/> Instance
    /// </summary>
    public BuildMenu BuildMenuInstance { get; }

    /// <summary>
    /// Public <see cref="PlanScreen"/> Instance
    /// </summary>
    public PlanScreen PlanScreenInstance { get; }

    /// <summary>
    /// Public <see cref="DebugPaintElementScreen"/> Instance
    /// </summary>
    public DebugPaintElementScreen DebugPaintElementScreenInstance { get; }

    /// <summary>
    /// Public <see cref="PriorityScreen"/> Instance
    /// </summary>
    public PriorityScreen PriorityScreenInstance { get; private init; }

    /// <summary>
    /// Public <see cref="ProductInfoScreen"/> Instance
    /// </summary>
    public ProductInfoScreen ProductInfoScreenInstance { get; init; }

    /// <summary>
    /// Public <see cref="MaterialSelectionPanel"/> Instance
    /// </summary>
    public MaterialSelectionPanel MaterialSelectionPanelInstance { get; init; }

    /// <summary>
    /// Declare that our Context is Fake or not.
    /// </summary>
    public bool IsFake { get; private init; }

    private StaticContext(
        ToolMenu toolMenu,
        BuildMenu buildMenu,
        PlanScreen planScreen,
        DebugPaintElementScreen debugPaintElementScreen
    )
    {
        ToolMenuInstance = toolMenu;
        BuildMenuInstance = buildMenu;
        PlanScreenInstance = planScreen;
        DebugPaintElementScreenInstance = debugPaintElementScreen;
    }

    private static StaticContext originalContext;
    private static readonly StaticContext overridden = Create();

    /// <summary>
    /// Current Static Context.
    /// </summary>
    public static StaticContext Current => originalContext != null ? overridden : GetCurrent();

    /// <summary>
    /// Override Current Context to Original.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void Override()
    {
        if (originalContext != null)
            throw new Exception("Unable to override already overridden static context");

        originalContext = GetCurrent();
        overridden.Switch();
    }

    /// <summary>
    /// Restore our fake context.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void Restore()
    {
        if (originalContext == null)
            throw new Exception("Unable to restore non-overridden static context");

        originalContext.Switch();
        originalContext = null;
    }

    private static StaticContext GetCurrent() => new(
        ToolMenu.Instance,
        BuildMenu.Instance,
        PlanScreen.Instance,
        DebugPaintElementScreen.Instance
    )
    {
        IsFake = false
    };

    private static StaticContext Create()
    {
        var context = new StaticContext(
            UnityObjectManager.CreateStub<ToolMenu>(),
            UnityObjectManager.CreateStub<BuildMenu>(),
            UnityObjectManager.CreateStub<PlanScreen>(),
            UnityObjectManager.CreateStub<DebugPaintElementScreen>()
        )
        {
            PriorityScreenInstance = UnityObjectManager.CreateStub<PriorityScreen>(),
            ProductInfoScreenInstance = UnityObjectManager.CreateStub<ProductInfoScreen>(),
            MaterialSelectionPanelInstance = UnityObjectManager.CreateStub<MaterialSelectionPanel>(),
            IsFake = true,
        };
        context.ToolMenuInstance.priorityScreen = context.PriorityScreenInstance;
        context.BuildMenuInstance.productInfoScreen = context.ProductInfoScreenInstance;
        context.PlanScreenInstance.ProductInfoScreen = context.ProductInfoScreenInstance;
        context.ProductInfoScreenInstance.materialSelectionPanel = context.MaterialSelectionPanelInstance;
        context.MaterialSelectionPanelInstance.priorityScreen = context.PriorityScreenInstance;

        context.DebugPaintElementScreenInstance.paintElement = new Toggle();
        context.DebugPaintElementScreenInstance.paintMass = new Toggle();
        context.DebugPaintElementScreenInstance.paintTemperature = new Toggle();
        context.DebugPaintElementScreenInstance.paintDiseaseCount = new Toggle();
        context.DebugPaintElementScreenInstance.paintDisease = new Toggle();
        context.DebugPaintElementScreenInstance.affectBuildings = new Toggle();
        context.DebugPaintElementScreenInstance.affectCells = new Toggle();

        return context;
    }

    private void Switch()
    {
        ToolMenu.Instance = ToolMenuInstance;
        BuildMenu.Instance = BuildMenuInstance;
        PlanScreen.Instance = PlanScreenInstance;
        DebugPaintElementScreen.Instance = DebugPaintElementScreenInstance;
    }
}
