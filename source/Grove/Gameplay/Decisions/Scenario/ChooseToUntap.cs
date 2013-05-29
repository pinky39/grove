namespace Grove.Gameplay.Decisions.Scenario
{
  public class ChooseToUntap : Decisions.ChooseToUntap, IScenarioDecision
  {
    public bool CanExecute()
    {
      return true;
    }

    protected override void ExecuteQuery() {}
  }
}