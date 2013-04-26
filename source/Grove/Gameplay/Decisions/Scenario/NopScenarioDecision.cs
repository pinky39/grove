namespace Grove.Gameplay.Decisions.Scenario
{
  using Player;

  public class NopScenarioDecision : IDecision
  {
    public bool HasCompleted { get { return true; } }
    public bool WasPriorityPassed { get { return true; } }

    public void Initialize(Player controller, Game game) {}

    public void Execute() {}
  }
}