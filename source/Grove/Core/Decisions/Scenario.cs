namespace Grove.Decisions
{
  using System.Collections.Generic;
  using System.Linq;

  public class Scenario
  {
    private readonly Game _game;
    private readonly List<ScenarioStep> _steps = new List<ScenarioStep>();

    public Scenario(Game game)
    {
      _game = game;
    }

    public void Define(params ScenarioStep[] steps)
    {
      foreach (var scenarioStep in steps)
      {
        _steps.Add(scenarioStep);
      }
    }

    public Verify GetVerify()
    {     
      return GetResult<Verify>(null);
    }

    public TResult GetResult<TResult>(Player controller) where TResult : class
    {
      var results = GetResults();

      if (results == null)
      {
        return null;
      }

      return results.PopNext<TResult>(controller);
    }

    private ScenarioStep GetResults() 
    {
      return _steps.SingleOrDefault(x => 
        x.Step == _game.Turn.Step && x.Turn == _game.Turn.TurnCount);
    }
  }
}