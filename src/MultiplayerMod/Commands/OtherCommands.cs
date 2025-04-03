using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Extensions;
using MultiplayerMod.Patches;
using MultiplayerMod.Patches.ScreenPatches;

namespace MultiplayerMod.Commands;

internal class OtherCommands
{
    internal static void SetDisinfectSettingsCommand_Event(SetDisinfectSettingsCommand playerCommand)
    {
        SaveGame.Instance.enableAutoDisinfect = playerCommand.EnableAutoDisinfect;
        SaveGame.Instance.minGermCountForDisinfect = playerCommand.MinGerm;
    }

    internal static void AcceptDeliveryCommand_Event(AcceptDeliveryCommand command)
    {
        TelepadPatch.IsRequestedByCommand = true;
        var telepad = command.Target.Resolve();
        telepad.OnAcceptDelivery(command.Deliverable);
        var capture = TelepadPatch.AcceptedGameObject;

        var minionMultiplayer = capture.GetComponent<MultiplayerInstance>();
        minionMultiplayer.Register(command.GameObjectId);

        var minionIdentity = capture.GetComponent<MinionIdentity>();
        if (minionIdentity != null)
        {
            var proxyMultiplayer = minionIdentity.GetMultiplayerInstance();
            proxyMultiplayer.Register(command.ProxyId);
        }

        ImmigrantScreenPatch.Deliverables = null;
    }

    internal static void RejectDeliveryCommand_Event(RejectDeliveryCommand command)
    {
        TelepadPatch.IsRequestedByCommand = true;
        command.Target.Resolve().RejectAll();
        ImmigrantScreenPatch.Deliverables = null;
    }

    internal static void ResearchEntryCommand_Event(ResearchEntryCommand command)
    {
        var screen = ManagementMenu.Instance.researchScreen;
        var entry = screen.entryMap.Values.FirstOrDefault(entry => entry.targetTech.Id == command.TechId);
        if (entry == null)
        {
            Debug.LogWarning($"Tech {command.TechId} is not found.");
            return;
        }
        if (command.IsCancel)
        {
            entry.OnResearchCanceled();
        }
        else
        {
            entry.OnResearchClicked();
        }
    }

    internal static void PermitConsumableByDefaultCommand_Event(PermitConsumableByDefaultCommand command)
    {
        ConsumerManager.instance.DefaultForbiddenTagsList.Clear();
        ConsumerManager.instance.DefaultForbiddenTagsList.AddRange(command.PermittedList);
        var screen = ManagementMenu.Instance.consumablesScreen;
        foreach (var row in screen.rows.Where(row => row.rowType == TableRow.RowType.Default))
        {
            foreach (var widget in row.widgets.Where(entry => entry.Key is ConsumableInfoTableColumn).Select(entry => entry.Value))
            {
                screen.on_load_consumable_info(null, widget);
            }
        }
    }

    internal static void PermitConsumableToMinionCommand_Event(PermitConsumableToMinionCommand command)
    {
        ConsumableConsumerPatch.IsCommandSent = true;
        var consumableConsumer = command.Instance.Resolve().GetComponent<ConsumableConsumer>();
        if (consumableConsumer == null) return;

        consumableConsumer.SetPermitted(command.ConsumableId, command.IsAllowed);
        var screen = ManagementMenu.Instance.consumablesScreen;
        foreach (var row in screen.rows)
        {
            var minion = row.GetIdentity();
            row.widgets.
                Where(entry => entry.Key is ConsumableInfoTableColumn).
                Select(entry => entry.Value).
                ForEach(widget => screen.on_load_consumable_info(minion, widget));
        }
    }

    internal static void ChangeSchedulesListCommand_Event(ChangeSchedulesListCommand command)
    {
        var manager = ScheduleManager.Instance;
        var schedules = manager.schedules;

        for (var i = 0; i < Math.Min(command.SerializableSchedules.Count, schedules.Count); i++)
        {
            ScheduleScreenEventsPatch.IsCommandSent = true;
            var schedule = schedules[i];
            var changedSchedule = command.SerializableSchedules[i];
            schedule.name = changedSchedule.Name;
            schedule.alarmActivated = changedSchedule.AlarmActivated;
            schedule.assigned = changedSchedule.Assigned;
            schedule.SetBlocksToGroupDefaults(changedSchedule.Groups); // Triggers "Changed"
        }

        if (Math.Abs(command.SerializableSchedules.Count - schedules.Count) > 1)
            Debug.LogWarning("Schedules update contains more than one schedule addition / removal");

        if (command.SerializableSchedules.Count > schedules.Count)
        {
            ScheduleScreenEventsPatch.IsCommandSent = true;
            // New schedules was added
            var newSchedule = command.SerializableSchedules.Last();
            var schedule = manager.AddSchedule(newSchedule.Groups, newSchedule.Name, newSchedule.AlarmActivated);
            schedule.assigned = newSchedule.Assigned;
            schedule.Changed();
        }
        else if (schedules.Count > command.SerializableSchedules.Count)
        {
            ScheduleScreenEventsPatch.IsCommandSent = true;
            // A schedule was removed
            manager.DeleteSchedule(schedules.Last());
        }
    }

    internal static void MasterSkillCommand_Event(MasterSkillCommand command)
    {
        var component = command.MinionIdentityReference.Resolve().GetComponent<MinionResume>();
        if (component == null)
            return;

        if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
            component.ForceAddSkillPoint();
        var masteryConditions = component.GetSkillMasteryConditions(command.SkillId);
        if (!component.CanMasterSkill(masteryConditions))
            return;
        if (component.HasMasteredSkill(command.SkillId))
            return;

        component.MasterSkill(command.SkillId);

        ManagementMenu.Instance.skillsScreen.RefreshAll();
    }

    internal static void SetHatCommand_Event(SetHatCommand command)
    {
        var resume = command.MinionIdentityReference.Resolve().GetComponent<MinionResume>();
        if (resume == null)
            return;

        resume.SetHats(resume.currentHat, command.TargetHat);
        if (command.TargetHat != null)
        {
            if (resume.OwnsHat(command.TargetHat))
            {
                var unused = new PutOnHatChore(resume, Db.Get().ChoreTypes.SwitchHat);
            }
        }
        else
        {
            resume.ApplyTargetHat();
        }

        ManagementMenu.Instance.skillsScreen.RefreshAll();
    }

    internal static void ChangeRedAlertStateCommand_Event(ChangeRedAlertStateCommand command)
    {
        ClusterManager.Instance.activeWorld.AlertManager.ToggleRedAlert(command.IsEnabled);
    }

    internal static void UpdateAlarmCommand_Event(UpdateAlarmCommand command)
    {
        var alarm = command.Args.Target.Resolve();
        alarm.notificationName = command.Args.NotificationName;
        alarm.notificationTooltip = command.Args.NotificationTooltip;
        alarm.pauseOnNotify = command.Args.PauseOnNotify;
        alarm.zoomOnNotify = command.Args.ZoomOnNotify;
        alarm.notificationType = command.Args.NotificationType;

        alarm.UpdateNotification(true);
    }

    internal static void UpdateLogicCounterCommand_Event(UpdateLogicCounterCommand command)
    {
        var logicCounter = command.Target.Resolve();
        logicCounter.maxCount = command.MaxCount;
        logicCounter.currentCount = command.CurrentCount;
        logicCounter.advancedMode = command.AdvancedMode;

        logicCounter.SetCounterState();
        logicCounter.UpdateLogicCircuit();
        logicCounter.UpdateVisualState(true);
        logicCounter.UpdateMeter();
    }

    internal static void UpdateCritterSensorCommand_Event(UpdateCritterSensorCommand command)
    {
        var logicCritterCountSensor = command.Target.Resolve();
        logicCritterCountSensor.countCritters = command.CountCritters;
        logicCritterCountSensor.countEggs = command.CountEggs;
    }

    internal static void UpdateLogicTimeOfDaySensorCommand_Event(UpdateLogicTimeOfDaySensorCommand command)
    {
        var sensor = command.Target.Resolve();
        sensor.startTime = command.StartTime;
        sensor.duration = command.Duration;
    }

    internal static void UpdateLogicTimeSensorCommand_Event(UpdateLogicTimeSensorCommand command)
    {
        var sensor = command.Target.Resolve();
        sensor.displayCyclesMode = command.DisplayCyclesMode;
        sensor.onDuration = command.OnDuration;
        sensor.offDuration = command.OffDuration;
        sensor.timeElapsedInCurrentState = command.TimeElapsedInCurrentState;
    }

    internal static void UpdateRailGunCapacityCommand_Event(UpdateRailGunCapacityCommand command)
    {
        var railGun = command.Target.Resolve();
        railGun.launchMass = command.LaunchMass;
    }

    internal static void UpdateTemperatureSwitchCommand_Event(UpdateTemperatureSwitchCommand command)
    {
        var temperatureControlledSwitch = command.Target.Resolve();
        temperatureControlledSwitch.thresholdTemperature = command.ThresholdTemperature;
        temperatureControlledSwitch.activateOnWarmerThan = command.ActivateOnWarmerThan;
    }
}
