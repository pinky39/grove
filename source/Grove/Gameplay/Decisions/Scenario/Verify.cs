namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using Misc;

  [Serializable]
  public class Verify : GameObject, IScenarioDecision
  {
    public Action Assertion { get; set; }

    public bool HasCompleted { get; private set; }
    public bool WasPriorityPassed { get { return true; } }

    public bool CanExecute()
    {
      return Stack.IsEmpty;
    }

    public void Initialize(Player controller, Game game)
    {
      Game = game;
    }

    public void Execute()
    {
      Assertion();
      HasCompleted = true;
    }
  }
}