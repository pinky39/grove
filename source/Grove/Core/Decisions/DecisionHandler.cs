namespace Grove.Decisions
{
  public abstract class DecisionHandler<TDecision, TResult> : GameObject, IDecisionHandler 
    where TResult : class 
    where TDecision : Decision
  {
    protected TDecision D;
    protected TResult Result;
    private bool _hasCompleted;

    protected virtual bool ShouldExecuteQuery { get { return true; } }
    public virtual bool HasCompleted { get { return _hasCompleted; } }
    public virtual bool IsPass { get { return false; } }

    public virtual void Execute()
    {
      if (ShouldExecuteQuery)
      {
        ExecuteQuery();
      }
      else
      {
        SetResultNoQuery();
      }

      ProcessResults();
      _hasCompleted = true;
    }

    public virtual void SaveDecisionResults()
    {
      SaveDecisionResult(Result);
    }

    public IDecisionHandler Initialize(object decision, Game game)
    {
      Initialize((TDecision) decision, game);
      return this;
    }

    public void ExecuteAssertions()
    {
      if (Stack.IsEmpty && !Stack.HasTriggered)
      {
        var verify = Game.Scenario.GetVerify();
        if (verify != null)
        {
          verify.Assertion();
        }
      }
    }
    
    public TResult GetNextScenarioResult()
    {      
      return Game.Scenario.GetResult<TResult>(D.Controller);
    }

    public abstract void ProcessResults();

    private void Initialize(TDecision decision, Game game)
    {
      Game = game;
      D = decision;

      Initialize();
    }

    protected virtual void Initialize() {}
    protected abstract void ExecuteQuery();
    protected virtual void SetResultNoQuery() {}
  }
}