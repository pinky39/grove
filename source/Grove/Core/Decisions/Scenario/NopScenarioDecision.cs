namespace Grove.Core.Decisions.Scenario
{
  public class NopScenarioDecision : IDecision
  {
    public void Init() {}

    public bool HasCompleted { get { return true; } }
    public bool WasPriorityPassed { get { return true; } }
    public Player Controller { get; set; }
    public Game Game { get; set; }

    public void Execute() {}
  }
}