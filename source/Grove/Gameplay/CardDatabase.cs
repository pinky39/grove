namespace Grove.Gameplay
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Misc;

  public class CardDatabase : IEnumerable<Card>
  {
    private readonly CardsSource[] _cardSources;
    private List<CardFactory> _database;
    private Dictionary<string, Card> _previews;

    public CardDatabase(CardsSource[] cardSources)
    {
      _cardSources = cardSources;
    }

    public Card this[string name] { get { return _previews[name.ToLowerInvariant()]; } }
    private IEnumerable<CardFactory> Database { get { return _database ?? (_database = CreateDatabase()); } }
    public int CardCount { get { return _cardSources.Length; } }

    public IEnumerator<Card> GetEnumerator()
    {
      return _previews.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public Card CreateCard(string name)
    {
      var cardFactory = GetCardFactory(name);
      return cardFactory.CreateCard();
    }

    private CardFactory GetCardFactory(string name)
    {
      var cardFactory = Database
        .Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        .FirstOrDefault();

      if (cardFactory == null)
        throw new InvalidOperationException(
          String.Format("Card with name '{0}' was not found in database.", name));
      return cardFactory;
    }

    public List<string> GetCardNames()
    {
      return Database.Select(x => x.Name)
        .Where(x => !x.Equals("Uncastable", StringComparison.InvariantCultureIgnoreCase))
        .OrderBy(x => x)
        .ToList();
    }

    private List<CardFactory> CreateDatabase()
    {
      var database = new List<CardFactory>();

      foreach (var cardSource in _cardSources)
      {
        database.AddRange(cardSource.GetCards());
      }

      return database;
    }

    public CardDatabase LoadPreviews()
    {
      _previews = Database
        .Select(x => x.CreateCard())
        .Where(x => !x.Is("uncastable"))
        .ToDictionary(x => x.Name.ToLowerInvariant());

      return this;
    }
  }
}