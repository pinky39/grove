namespace Grove.Core.Decisions.Scenario
{
  using Gameplay.Decisions.Results;

  public class DeclareAttackers : Gameplay.Decisions.DeclareAttackers, IScenarioDecision
  {
    public static DeclareAttackers None
    {
      get
      {
        return new DeclareAttackers
          {
            Result = new ChosenCards()
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