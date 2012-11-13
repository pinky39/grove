namespace Grove.Core.Decisions.Machine
{
  using Grove.Core.Ai;

  public class ChooseToUntap : Decisions.ChooseToUntap, ISearchNode, IDecisionExecution
  {
    private DecisionExecutor _executor;

    public ChooseToUntap()
    {
      Result = false;
    }

    public Search Search { get { return Game.Search; } }
    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }
    public override bool HasCompleted { get { return _executor.HasCompleted; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    public int ResultCount { get { return 2; } }

    public void SetResult(int index)
    {
      Result = index == 0 ? false : true;
    }

    public void GenerateChoices() {}

    public override void Execute()
    {
      _executor.Execute();
    }

    public override void Init()
    {
      _executor = new DecisionExecutor(this, Game.ChangeTracker);
    }

    protected override void ExecuteQuery()
    {
      Search.SetBestResult(this);
    }
  }
}