namespace Grove.Core.Controllers.Scenario
{
  public class DefaultScenarioDecision : IDecision
  {
    public void Init() {}

    public bool HasCompleted { get { return true; } }
    public bool WasPriorityPassed { get { return true; } }
    public Player Controller { get; set; }
    public Game Game { get; set; }

    public void Execute() {}
  }
}