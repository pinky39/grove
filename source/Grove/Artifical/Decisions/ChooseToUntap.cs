namespace Grove.Artifical.Decisions
{
  using Gameplay;

  public class ChooseToUntap : Gameplay.Decisions.ChooseToUntap, ISearchNode, IDecisionExecution
  {
    private readonly DecisionExecutor _executor;

    public ChooseToUntap()
    {      
      _executor = new DecisionExecutor(this);
      Result = false;
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }
    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    Game ISearchNode.Game { get { return Game; } }

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

    public override void Initialize(Player controller, Game game)
    {
      base.Initialize(controller, game);
      _executor.Initialize(game.ChangeTracker);
    }

    protected override void ExecuteQuery()
    {
      Ai.SetBestResult(this);
    }
  }
}