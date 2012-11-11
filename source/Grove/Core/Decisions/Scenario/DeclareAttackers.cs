namespace Grove.Core.Controllers.Scenario
{
  using System;
  using Results;

  public class DeclareAttackers : Controllers.DeclareAttackers, IScenarioDecision
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