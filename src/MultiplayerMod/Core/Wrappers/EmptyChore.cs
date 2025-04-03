namespace MultiplayerMod.Core.Wrappers;

internal class EmptyChore : Chore<EmptyChore.StatesInstance>
{
    public EmptyChore(IStateMachineTarget target) :
        base(Db.Get().ChoreTypes.Idle,
        target,
        target.GetComponent<ChoreProvider>(),
        run_until_complete: false,
        null,
        null,
        null,
        PriorityScreen.PriorityClass.basic,
        5,
        is_preemptable: false,
        allow_in_context_menu: true,
        0,
        add_to_daily_report: false,
        ReportManager.ReportType.WorkTime)
    {
        base.smi = new StatesInstance(this);
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, EmptyChore, object>.GameInstance
    {

        public StatesInstance(EmptyChore master) : base(master)
        {

        }


    }

    public class States : GameStateMachine<States, StatesInstance, EmptyChore>
    {
        public static State EmptyState = new();

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = EmptyState;
            EmptyState.DoNothing();
        }
    }
}
