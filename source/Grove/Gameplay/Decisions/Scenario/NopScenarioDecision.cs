namespace Grove.Core.Decisions.Scenario
{
  using Gameplay;
  using Gameplay.Decisions;
  using Gameplay.Player;

  public class NopScenarioDecision : IDecision
  {
    public bool HasCompleted { get { return true; } }
    public bool WasPriorityPassed { get { return true; } }

    public void Initialize(Player controller, Game game) {}

    public void Execute() {}
  }
}