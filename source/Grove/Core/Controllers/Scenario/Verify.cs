namespace Grove.Core.Controllers.Scenario
{
  using System;
  using Infrastructure;
  using Zones;

  public class Verify : IScenarioDecision
  {
    public Action Assertion { get; set; }
    public Stack Stack { get; set; }
    public bool HasCompleted { get; private set; }
    public bool WasPriorityPassed { get { return true; } }


    public bool CanExecute()
    {
      return Stack.IsEmpty;
    }

    public void Execute()
    {
      Assertion();
      HasCompleted = true;
    }

    public IDecision Init(ChangeTracker changeTracker)
    {
      return this;
    }
  }
}