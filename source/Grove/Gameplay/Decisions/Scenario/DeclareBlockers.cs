namespace Grove.Core.Decisions.Scenario
{
  using Gameplay.Decisions.Results;

  public class DeclareBlockers : Gameplay.Decisions.DeclareBlockers, IScenarioDecision
  {
    public static DeclareBlockers None
    {
      get
      {
        return new DeclareBlockers
          {
            Result = new ChosenBlockers()
          };
      }
    }

    protected override bool ShouldExecuteQuery { get { return true; } }

    public bool CanExecute()
    {
      return true;
    }

    protected override void ExecuteQuery() {}
  }
}