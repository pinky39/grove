namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using Decisions;

  public class MatchSimulator
  {
    private readonly CardDatabase _cardDatabase;
    private readonly DecisionSystem _decisionSystem;

    public MatchSimulator(CardDatabase cardDatabase, DecisionSystem decisionSystem)
    {
      _cardDatabase = cardDatabase;
      _decisionSystem = decisionSystem;
    }

    public SimulationResult Simulate(Deck deck1, Deck deck2)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var result = new SimulationResult();

      while (result.Deck1WinCount < 2 && result.Deck2WinCount < 2)
      {
        SimulateGame(deck1, deck2, result);
      }

      stopwatch.Stop();

      result.Duration = stopwatch.Elapsed;

      return result;
    }

    private void SimulateGame(Deck deck1, Deck deck2, SimulationResult result)
    {
      var game = Game.NewSimulation(deck1, deck2, _cardDatabase, _decisionSystem);      
            
      game.Start();

      if (game.Players.BothHaveLost)
        return;

      if (game.Players.Player1.HasLost)
      {
        result.Deck2WinCount++;
        return;
      }

      result.Deck1WinCount++;
      return;
    }  

    public class SimulationResult
    {
      public int Deck1WinCount { get; set; }
      public int Deck2WinCount { get; set; }
      public TimeSpan Duration { get; set; }
    }
  }
}