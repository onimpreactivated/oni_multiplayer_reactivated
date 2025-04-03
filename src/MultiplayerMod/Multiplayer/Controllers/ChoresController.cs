using MultiplayerMod.ChoreSync;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Wrappers;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Multiplayer.Controllers;

internal static class ChoresController
{
    public static ConditionalWeakTable<ChoreDriver, BoxedValue<bool>> driversAvailability = new();
    public static List<Type> supportedTypes = [];//[..ChoreSyncList.GetSyncTypes()];

    public static bool Supported(Chore chore) => supportedTypes.Contains(chore.GetType());

    public static Chore.Precondition IsDriverBusy = new()
    {
        id = nameof(IsDriverBusy),
        description = "The chore driver is busy with a host chore",
        fn = (ref Chore.Precondition.Context context, object _) => !Busy(ref context),
        sortOrder = -1
    };

    public static Chore.Precondition IsMultiplayerChore = new()
    {
        id = nameof(IsMultiplayerChore),
        description = "The chore is created in multiplayer and will be executed manually",
        fn = (ref Chore.Precondition.Context context, object _) => MultiplayerManager.Instance.MPObjects.Get(context.chore) == null,
        sortOrder = -1
    };

    private static bool Busy(ref Chore.Precondition.Context context) =>
    driversAvailability.TryGetValue(
        context.consumerState.choreDriver,
        out var result
    ) && result.Value;

    public static void Set(ChoreDriver driver, ref Chore.Precondition.Context context)
    {
        var busy = driversAvailability.GetValue(driver, _ => new BoxedValue<bool>(true));
        busy.Value = true;
        driver.SetChore(context);
    }

    public static void Release(ChoreDriver driver)
    {
        var busy = driversAvailability.GetValue(driver, _ => new BoxedValue<bool>(false));
        busy.Value = false;
    }
}
