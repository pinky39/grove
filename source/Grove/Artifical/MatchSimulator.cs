namespace Grove.Artifical
{
  using System;
  using System.Diagnostics;
  using Gameplay;
  using Gameplay.Decisions;

  public class MatchSimulator
  {
    private readonly CardsDatabase _cardsDatabase;
    private readonly DecisionSystem _decisionSystem;

    public MatchSimulator(CardsDatabase cardsDatabase, DecisionSystem decisionSystem)
    {
      _cardsDatabase = cardsDatabase;
      _decisionSystem = decisionSystem;
    }

    public SimulationResult Simulate(Deck deck1, Deck deck2, int maxTurnsPerGame = 100,
      int maxSearchDepth = 16, int maxTargetsCount = 2)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var result = new SimulationResult();

      while (result.Deck1WinCount < 2 && result.Deck2WinCount < 2)
      {
        SimulateGame(deck1, deck2, result, maxTurnsPerGame, maxSearchDepth, maxTargetsCount);
      }

      stopwatch.Stop();

      result.Duration = stopwatch.Elapsed;

      return result;
    }

    private void SimulateGame(Deck deck1, Deck deck2, SimulationResult result, int maxTurnsPerGame,
      int maxSearchDepth, int maxTargetsCount)
    {
      var stopwatch = new Stopwatch();
      var game = Game.NewSimulation(
        deck1,
        deck2,
        maxSearchDepth,
        maxTargetsCount,
        _cardsDatabase,
        _decisionSystem);

      game.Ai.SearchStarted += delegate
        {
          result.TotalSearchCount++;
          stopwatch.Start();
        };

      game.Ai.SearchFinished += delegate
        {
          stopwatch.Stop();

          if (stopwatch.Elapsed > result.MaxSearchTime)
          {
            result.MaxSearchTime = stopwatch.Elapsed;
          }

          stopwatch.Reset();
        };

      game.Start(numOfTurns: maxTurnsPerGame);

      result.TotalTurnCount += game.Turn.TurnCount;

      if (game.Players.BothHaveLost)
        return;

      if (game.Players.Player1.Score > -game.Players.Player2.Score)
      {
        result.Deck1WinCount++;
        return;
      }

      result.Deck2WinCount++;
      return;
    }

    public class SimulationResult
    {
      public int Deck1WinCount { get; set; }
      public int Deck2WinCount { get; set; }
      public TimeSpan Duration { get; set; }
      public int TotalTurnCount { get; set; }
      public int TotalSearchCount { get; set; }
      public TimeSpan MaxSearchTime { get; set; }
    }
  }
}