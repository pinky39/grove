namespace Grove.Core
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Infrastructure;

  public class DeckEvaluator
  {
    private readonly MatchSimulator _matchSimulator;

    public DeckEvaluator(MatchSimulator matchSimulator)
    {
      _matchSimulator = matchSimulator;
    }

    public List<string> GetBestDeck(List<List<string>> decks)
    {
      var bestDecks = decks.Select((x, i) => new Deck {Number = i + 1, Cards = x}).Shuffle().ToList();

      int round = 1;
      while (bestDecks.Count > 1)
      {
        Deck bye = null;

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

      return bestDecks[0].Cards;
    }

    private List<Deck> PlayOneRound(List<Deck> decks)
    {            
      var tasks = new List<Task>();
      var winners = new ConcurrentBag<Deck>();

      for (var i = 0; i < decks.Count; i = i + 2)
      {
        var deck1 = decks[i];
        var deck2 = decks[i + 1];

        //var task = Task.Factory.StartNew(() =>
        //  {            
            Console.WriteLine("Deck {0} is playing against Deck {1}...", deck1.Number, deck2.Number);
            var result = _matchSimulator.Simulate(
              deck1.Cards, 
              deck2.Cards, 
              maxTurnsPerGame: 25,
              maxSearchDepth: 12,
              maxTargetsCount: 2);

            if (result.Deck1WinCount > result.Deck2WinCount)
            {
              Console.WriteLine("Deck {0} wins against Deck {1} ({2}-{3}).", deck1.Number, deck2.Number, result.Deck1WinCount, result.Deck2WinCount);
              winners.Add(deck1);
            }
            else
            {
              Console.WriteLine("Deck {0} wins against Deck {1}. ({2}-{3}).", deck2.Number, deck1.Number, result.Deck2WinCount, result.Deck1WinCount);
              winners.Add(deck2);
            }
          //});

        //tasks.Add(task);
      }

      //Task.WaitAll(tasks.ToArray());
      return winners.ToList();
    }

    private class Deck
    {
      public List<string> Cards;
      public int Number;
    }
  }
}