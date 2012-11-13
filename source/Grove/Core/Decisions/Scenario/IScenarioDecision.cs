namespace Grove.Core.Decisions.Scenario
{
  public interface IScenarioDecision : IDecision
  {
    bool CanExecute();
  }
}