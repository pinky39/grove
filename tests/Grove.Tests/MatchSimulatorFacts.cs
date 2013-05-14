﻿namespace Grove.Tests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Persistance;
  using Xunit;

  public class MatchSimulatorFacts : Scenario
  {
    [Fact]
    public void Simulate()
    {
      var deck1 = "deck1.dec";
      var deck2 = "deck2.dec";
      
      var result = MatchSimulator.Simulate(GetDeck(deck1), GetDeck(deck2),
        maxTurnsPerGame: 25, maxSearchDepth: 12, maxTargetsCount: 2);

      Console.WriteLine(@"{0} vs {1}", deck1, deck2);
      Console.WriteLine(@"{0} win count: {1}.", deck1, result.Deck1WinCount);
      Console.WriteLine(@"{0} win count: {1}.", deck2, result.Deck2WinCount);
      Console.WriteLine(@"Match duration: {0}.", result.Duration);
      Console.WriteLine(@"Turn count: {0}.", result.TotalTurnCount);
      Console.WriteLine(@"Total search count: {0}.", result.TotalSearchCount);
      Console.WriteLine(@"Max search time: {0}.", result.MaxSearchTime);

      Assert.True(result.Deck1WinCount + result.Deck2WinCount >= 2);
    }

    private List<string> GetDeck(string name)
    {
      return new DeckIo(CardsInfo).Read(@".\" + name).ToList();
    }
  }
}