using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Execution;

internal class ExecutionManager
{
    internal static ExecutionLevel PrevLevel { get; set; }
    internal static ExecutionLevel CurrentLevel { get; set; } = ExecutionLevel.System;


    /// <summary>
    /// Overrides execution level for subsequent code execution.
    /// Must be accompanied with <see cref="LeaveOverrideSection"/>.
    /// </summary>
    /// <param name="level">An execution level that will be set to current code execution</param>
    /// <param name="memberName"></param>
    public static void EnterOverrideSection(ExecutionLevel level, [CallerMemberName] string memberName = "")
    {
        Debug.Log($"Overriding execution level {CurrentLevel} -> {level} {memberName}");
        PrevLevel = CurrentLevel;
        CurrentLevel = level;
    }

    /// <summary>
    /// Restores overridden execution level to its previous value.
    /// This should be called after each call to <see cref="EnterOverrideSection"/>.
    /// </summary>
    /// <param name="memberName"></param>
    public static void LeaveOverrideSection([CallerMemberName] string memberName = "")
    {
        CurrentLevel = PrevLevel;
        Debug.Log($"Execution level was restored to {CurrentLevel} {memberName}");
    }

    /// <summary>
    /// Runs an action if level requirements are satisfied.
    /// </summary>
    /// <param name="requiredLevel">An execution level required to run the action</param>
    /// <param name="action">An action</param>
    public static void RunIfLevelIsActive(ExecutionLevel requiredLevel, System.Action action)
    {
        if (!LevelIsActive(requiredLevel))
            return;
        action();
    }

    /// <summary>
    /// Runs an action if level requirements are satisfied.
    /// </summary>
    /// <param name="requiredLevel">An execution level required to run the action</param>
    /// <param name="actionLevel">An execution level that will be active during action execution</param>
    /// <param name="action">An action</param>
    public static void RunIfLevelIsActive(ExecutionLevel requiredLevel, ExecutionLevel actionLevel, System.Action action)
    {
        if (!LevelIsActive(requiredLevel))
            return;
        RunUsingLevel(actionLevel, action);
    }

    /// <summary>
    /// Runs an action with overridden execution level.
    /// </summary>
    /// <param name="level">An execution level that will be active during action execution</param>
    /// <param name="action">An action</param>
    public static void RunUsingLevel(ExecutionLevel level, System.Action action)
    {
        try
        {
            EnterOverrideSection(level);
            action();
        }
        finally
        {
            LeaveOverrideSection();
        }
    }

    /// <summary>
    /// Checks whether the required execution level is active.
    /// </summary>
    /// <param name="requiredLevel">A required execution level</param>
    /// <returns></returns>
    public static bool LevelIsActive(ExecutionLevel requiredLevel)
    {
        bool isActive = CurrentLevel >= requiredLevel;
        return isActive;
    }
}
