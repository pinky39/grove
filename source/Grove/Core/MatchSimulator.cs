namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;

  public class MatchSimulator
  {
    private readonly Game.IFactory _gameFactory;
    private readonly Player.IFactory _playerFactory;

    public MatchSimulator(Game.IFactory gameFactory, Player.IFactory playerFactory)
    {
      _gameFactory = gameFactory;
      _playerFactory = playerFactory;
    }

    public SimulationResult Simulate(IEnumerable<string> deck1, IEnumerable<string> deck2)
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

    private void SimulateGame(IEnumerable<string> deck1, IEnumerable<string> deck2, SimulationResult result)
    {
      var game = _gameFactory.Create();

      game.Players.Player1 = Create("player1", deck1);
      game.Players.Player2 = Create("player2", deck2);

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

    private Player Create(string name, IEnumerable<string> deck)
    {
      return _playerFactory.Create(
        name: name,
        avatar: "avatar",
        type: PlayerType.Computer,
        deck: deck);
    }

    public class SimulationResult
    {
      public int Deck1WinCount { get; set; }
      public int Deck2WinCount { get; set; }
      public TimeSpan Duration { get; set; }
    }
  }
}