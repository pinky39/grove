namespace Grove.Gameplay.Decisions.Scenario
{
  public class NopScenarioDecision : IDecision
  {
    public bool HasCompleted { get { return true; } }
    public bool IsPass { get { return true; } }

    public void Initialize(Player controller, Game game) {}

    public void Execute() {}

    public void SaveDecisionResults() {}
  }
}