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

    public SimulationResult Simulate(List<string> deck1, List<string> deck2)
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

      result.WinningDeck = result.Deck1WinCount > result.Deck2WinCount ? deck1 : deck2;
      return result;
    }

    private void SimulateGame(List<string> deck1, List<string> deck2, SimulationResult result)
    {
      Game game = Game.NewSimulation(deck1, deck2, _cardDatabase, _decisionSystem);

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
      public List<string> WinningDeck { get; set; }
      public TimeSpan Duration { get; set; }
    }
  }
}