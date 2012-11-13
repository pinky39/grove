namespace Grove.Core.Decisions.Scenario
{
  public class ChooseToUntap : Decisions.ChooseToUntap, IScenarioDecision
  {
    protected override void ExecuteQuery()
    {      
    }

    public bool CanExecute()
    {
      return true;
    }
  }
}