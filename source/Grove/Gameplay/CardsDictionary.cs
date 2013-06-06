namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class CardsDictionary
  {
    private readonly Dictionary<string, Card> _cards = new Dictionary<string, Card>();
    private readonly Dictionary<string, List<Card>> _fulltextSearchDatabase = new Dictionary<string, List<Card>>();
    private static readonly char[] Separators = new[] {'.', ',', '!', ';', ':', ' ', '?', '\'', '"'};

    public CardsDictionary(CardsDatabase cardsDatabase)
    {
      _cards = cardsDatabase.CreateAll()
        .ToDictionary(x => x.Name.ToLowerInvariant(), x => x);
    }

    public int Count { get { return _cards.Count; } }
    public Card this[string name] { get { return _cards[name.ToLowerInvariant()]; } }

    public void CreateFulltextSearchDatabase()
    {
      foreach (var card in _cards.Values)
      {
        var keywords = CreateKeywords(card);

        foreach (var keyword in keywords)
        {
          List<Card> entry;

          if (_fulltextSearchDatabase.TryGetValue(keyword, out entry) == false)
          {
            entry = new List<Card>();
            _fulltextSearchDatabase[keyword] = entry;
          }

          entry.Add(card);
        }
      }
    }

    public IEnumerable<Card> Query(IList<string> keywords)
    {
      if (keywords.Count == 0)
        return _cards.Values;

      var queryResults = new HashSet<Card>();

      queryResults.UnionWith(Query(keywords[0]));

      foreach (var keyword in keywords.Skip(1))
      {
        queryResults.IntersectWith(Query(keyword));
      }

      return queryResults;
    }

    private IEnumerable<Card> Query(string keyword)
    {
      List<Card> results;

      var result = new List<Card>();
      if (_fulltextSearchDatabase.TryGetValue(keyword, out results))
      {
        foreach (var card in results)
        {
          result.Add(card);
        }
      }

      return result.Distinct();
    }

    private static IEnumerable<string> CreateKeywords(Card card)
    {
      foreach (var keyword in card.Name.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
      {        
        yield return keyword.ToLowerInvariant();
      }

      foreach (var keyword in card.Text.GetTextOnly())
      {
        var trimmed = keyword.Trim(Separators);
        yield return trimmed.ToLowerInvariant();
      }

      foreach (var keyword in card.Type.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
      {
        yield return keyword.ToLowerInvariant();
      }
    }

    public List<string> GetCardNames()
    {
      return _cards.Values.Select(x => x.Name).OrderBy(x => x).ToList();
    }
  }
}