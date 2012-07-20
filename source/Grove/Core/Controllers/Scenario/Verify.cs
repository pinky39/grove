namespace Grove.Core.Controllers.Scenario
{
  using System;

  public class Verify : IScenarioDecision
  {
    private Game _game;
    public Action Assertion { get; set; }


    public void Init(Game game, IPlayer controller)
    {
      _game = game;
    }

    public bool HasCompleted { get; private set; }
    public bool WasPriorityPassed { get { return true; } }

    public bool CanExecute()
    {
      return _game.Stack.IsEmpty;
    }

    public void Execute()
    {
      Assertion();
      HasCompleted = true;
    }
  }
}