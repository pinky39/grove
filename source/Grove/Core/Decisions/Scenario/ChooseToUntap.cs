namespace Grove.Core.Controllers.Scenario
{
  public class ChooseToUntap : Controllers.ChooseToUntap, IScenarioDecision
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