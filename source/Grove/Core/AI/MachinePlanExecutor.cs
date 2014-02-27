namespace Grove.AI
{
  using Grove.Infrastructure;

  [Copyable]
  public class MachinePlanExecutor
  {
    public enum State
    {
      BeforeQuery,
      AfterQuery,
      Completed
    }

    private readonly IMachineExecutionPlan _decision;
    private readonly Trackable<State> _state = new Trackable<State>(State.BeforeQuery);

    private MachinePlanExecutor() {}

    public MachinePlanExecutor(IMachineExecutionPlan decision)
    {
      _decision = decision;
    }

    private State DecisionState { get { return _state.Value; } set { _state.Value = value; } }
    public bool HasCompleted { get { return DecisionState == State.Completed; } }

    public virtual void Initialize(ChangeTracker changeTracker)
    {
      _state.Initialize(changeTracker);
    }

    public void Execute()
    {
      if (DecisionState == State.BeforeQuery)
      {
        DecisionState = State.AfterQuery;

        if (_decision.ShouldExecuteQuery == false)
        {
          DecisionState = State.Completed;
          return;
        }

        _decision.ExecuteQuery();
      }

      DecisionState = State.Completed;
      _decision.ProcessResults();
    }
  }
}