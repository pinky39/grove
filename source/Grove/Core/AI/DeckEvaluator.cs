namespace Grove.AI
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Grove.Infrastructure;

  public static class DeckEvaluator
  {
    public static Deck GetBestDeck(List<Deck> decks)
    {
      var bestDecks = decks
        .Select((x, i) => new NumberedDeck {Number = i + 1, Deck = x})
        .OrderBy(x => RandomEx.Next())
        .ToList();

      var round = 1;
      while (bestDecks.Count > 1)
      {
        NumberedDeck bye = null;

        if (bestDecks.Count%2 != 0)
        {
          bye = bestDecks[0];
          bestDecks.RemoveAt(0);
        }

        Console.WriteLine("Round {0}", round);
        Console.WriteLine("--------------------------------");

        bestDecks = PlayOneRound(bestDecks);

        Console.WriteLine();

        if (bye != null)
          bestDecks.Add(bye);

        round++;
      }

      return bestDecks[0].Deck;
    }

    private static List<NumberedDeck> PlayOneRound(List<NumberedDeck> decks)
    {
      var tasks = new List<Task>();
      var winners = new ConcurrentBag<NumberedDeck>();

      for (var i = 0; i < decks.Count; i = i + 2)
      {
        var deck1 = decks[i];
        var deck2 = decks[i + 1];

        var task = Task.Factory.StartNew(() =>
          {
            Console.WriteLine("Deck {0} is playing against Deck {1}...", deck1.Number, deck2.Number);
            var result = MatchSimulator.Simulate(
              deck1.Deck,
              deck2.Deck,
              maxTurnsPerGame: 15,
              maxSearchDepth: 20,
              maxTargetsCount: 1);

            if (result.Deck1WinCount > result.Deck2WinCount)
            {
              Console.WriteLine("Deck {0} wins against Deck {1} ({2}-{3}).", deck1.Number, deck2.Number,
                                result.Deck1WinCount, result.Deck2WinCount);
              winners.Add(deck1);
            }
            else
            {
              Console.WriteLine("Deck {0} wins against Deck {1}. ({2}-{3}).", deck2.Number, deck1.Number,
                                result.Deck2WinCount, result.Deck1WinCount);
              winners.Add(deck2);
            }
          }, TaskCreationOptions.LongRunning);

        tasks.Add(task);
      }

      Task.WaitAll(tasks.ToArray());
      return winners.ToList();
    }

    private class NumberedDeck
    {
      public Deck Deck;
      public int Number;
    }
  }
}