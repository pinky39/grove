namespace Grove.Core.Decisions.Scenario
{
  using Gameplay.Decisions;

  public interface IScenarioDecision : IDecision
  {
    bool CanExecute();
  }
}