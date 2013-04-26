namespace Grove.Gameplay.Decisions.Scenario
{
  public interface IScenarioDecision : IDecision
  {
    bool CanExecute();
  }
}