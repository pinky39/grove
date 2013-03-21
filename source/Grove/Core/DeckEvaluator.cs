namespace Grove.Core
{
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
      var bestDecks = decks.Shuffle().ToList();

      while (bestDecks.Count > 1)
      {
        List<string> bye = null;

        if (bestDecks.Count%2 != 0)
        {
          bye = bestDecks[0];
          bestDecks.RemoveAt(0);
        }

        bestDecks = PlayOneRound(bestDecks);

        if (bye != null)
          bestDecks.Add(bye);
      }

      return bestDecks[0];
    }

    private List<List<string>> PlayOneRound(List<List<string>> decks)
    {
      var tasks = new List<Task>();
      var winners = new ConcurrentBag<List<string>>();

      for (var i = 0; i < decks.Count; i = i + 2)
      {
        var deck1 = decks[i];
        var deck2 = decks[i + 1];

        var task = Task.Factory.StartNew(() =>
          {
            var result = _matchSimulator.Simulate(deck1, deck2);
            winners.Add(result.WinningDeck);
          });

        tasks.Add(task);
      }

      Task.WaitAll(tasks.ToArray());
      return winners.ToList();
    }
  }
}