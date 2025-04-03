using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches.ScreenPatches;

[HarmonyPatch(typeof(ImmigrantScreen))]
internal static class ImmigrantScreenPatch
{
    public static List<ITelepadDeliverable> Deliverables { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ImmigrantScreen.Initialize))]
    private static void Initialize(ImmigrantScreen __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        var createDelivers = new TaskFactory().StartNew(async () =>
        {
            if (Deliverables != null)
                return;

            var readyDeliverables = await ImmigrantScreenExtensions.WaitForAllDeliverablesReady(__instance);
            if (readyDeliverables != null)
            {
                MultiplayerManager.Instance.NetClient.Send(new InitializeImmigrationCommand(Deliverables));
                Deliverables = readyDeliverables;
            }
        });
        createDelivers.Wait();

        var setdeliveries = new TaskFactory().StartNew(async () =>
        {
            if (Deliverables == null)
                return;
            // Wait until default initialize is complete
            await ImmigrantScreenExtensions.WaitForAllDeliverablesReady(__instance);
            // Create correct containers.
            InitializeContainers(Deliverables, ImmigrantScreen.instance);
            // Wait until those containers are initialized with random data.
            await ImmigrantScreenExtensions.WaitForAllDeliverablesReady(__instance);

            SetDeliverablesData(Deliverables, __instance);
        });
        setdeliveries.Wait();
    }
    private static void InitializeContainers(
    List<ITelepadDeliverable> telepadDeliverables,
    CharacterSelectionController screen
)
    {
        screen.OnReplacedEvent = null;
        screen.containers?.ForEach(cc => UnityEngine.Object.Destroy(cc.GetGameObject()));
        screen.containers?.Clear();
        screen.containers = [];

        screen.numberOfCarePackageOptions = 0;
        screen.numberOfDuplicantOptions = 0;
        screen.selectedDeliverables = [];

        foreach (var deliverable in telepadDeliverables)
        {
            if (deliverable is MinionStartingStats)
            {
                var characterContainer = Util.KInstantiateUI<CharacterContainer>(
                    screen.containerPrefab.gameObject,
                    screen.containerParent
                );
                screen.containers.Add(characterContainer);
                characterContainer.SetController(screen);
                characterContainer.SetReshufflingState(false);
                screen.numberOfDuplicantOptions++;
            }
            else
            {
                var packageContainer = Util.KInstantiateUI<CarePackageContainer>(
                    screen.carePackageContainerPrefab.gameObject,
                    screen.containerParent
                );
                screen.containers.Add(packageContainer);
                packageContainer.SetController(screen);
                screen.numberOfCarePackageOptions++;
            }
        }
    }

    private static void SetDeliverablesData(
        List<ITelepadDeliverable> telepadDeliverables,
        CharacterSelectionController screen
    )
    {
        var minionStats = telepadDeliverables.OfType<MinionStartingStats>().ToArray();
        var packageData =
            telepadDeliverables.OfType<CarePackageContainer.CarePackageInstanceData>().ToArray();
        for (var i = 0; i < minionStats.Length; i++)
        {
            SetCharacterStats(screen.containers.OfType<CharacterContainer>().ToArray()[i], minionStats[i]);
        }
        for (var i = 0; i < packageData.Length; i++)
        {
            SetPackageData(screen.containers.OfType<CarePackageContainer>().ToArray()[i], packageData[i]);
        }
    }

    private static void SetCharacterStats(CharacterContainer characterContainer, MinionStartingStats stats)
    {
        // Based on CharacterContainer.GenerateCharacter
        characterContainer.stats = stats;
        characterContainer.SetAnimator();
        characterContainer.SetInfoText();
    }

    private static void SetPackageData(
        CarePackageContainer packageContainer,
        CarePackageContainer.CarePackageInstanceData data
    )
    {
        // Based on CharacterContainer.GenerateCharacter
        packageContainer.carePackageInstanceData = data;
        packageContainer.info = data.info;
        packageContainer.ClearEntryIcons();
        packageContainer.SetAnimator();
        packageContainer.SetInfoText();
    }
}
