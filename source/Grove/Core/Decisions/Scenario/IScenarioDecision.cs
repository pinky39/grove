namespace Grove.Core.Controllers.Scenario
{
  public interface IScenarioDecision : IDecision
  {
    bool CanExecute();
  }
}