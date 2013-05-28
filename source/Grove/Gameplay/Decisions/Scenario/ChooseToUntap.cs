namespace Grove.Gameplay.Decisions.Scenario
{
  using System;

  [Serializable]
  public class ChooseToUntap : Decisions.ChooseToUntap, IScenarioDecision
  {
    public bool CanExecute()
    {
      return true;
    }

    protected override void ExecuteQuery() {}
  }
}