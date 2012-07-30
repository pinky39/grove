namespace Grove.Core.Controllers.Scenario
{
  using System;

  public class Verify : IScenarioDecision
  {
    public Action Assertion { get; set; }

    public void Init() {}

    public bool HasCompleted { get; private set; }
    public bool WasPriorityPassed { get { return true; } }
    public Player Controller { get; set; }
    public Game Game { get; set; }

    public bool CanExecute()
    {
      return Game.Stack.IsEmpty;
    }

    public void Execute()
    {
      Assertion();
      HasCompleted = true;
    }
  }
}