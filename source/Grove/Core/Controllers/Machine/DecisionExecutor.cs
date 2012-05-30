namespace Grove.Core.Controllers.Machine
{
  using Infrastructure;

  [Copyable]
  public class DecisionExecutor
  {
    private readonly IDecisionExecution _decision;
    private readonly Trackable<State> _state;

    private DecisionExecutor() {}

    public DecisionExecutor(IDecisionExecution decision, ChangeTracker changeTracker)
    {
      _decision = decision;
      _state = new Trackable<State>(State.BeforeQuery, changeTracker);
    }

    private State DecisionState { get { return _state.Value; } set { _state.Value = value; } }
    public bool HasCompleted { get { return DecisionState == State.Completed; } }

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

    public enum State
    {
      BeforeQuery,
      AfterQuery,
      Completed
    }
  }
}