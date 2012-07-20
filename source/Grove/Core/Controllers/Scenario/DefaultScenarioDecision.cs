namespace Grove.Core.Controllers.Scenario
{
  public class DefaultScenarioDecision : IDecision
  {
    public void Init(Game game, IPlayer controller) {}

    public bool HasCompleted { get { return true; } }
    public bool WasPriorityPassed { get { return true; } }
    public void Execute() {}
  }
}