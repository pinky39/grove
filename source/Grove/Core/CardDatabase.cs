namespace Grove.Core
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Dsl;

  public class CardDatabase : IEnumerable<Card>
  {
    private readonly CardsSource[] _cardSources;
    private List<ICardFactory> _database;
    private Dictionary<string, Card> _previews;

    public CardDatabase(CardsSource[] cardSources)
    {
      _cardSources = cardSources;
    }

    public Card this[string name] { get { return _previews[name.ToLowerInvariant()]; } }
    private IEnumerable<ICardFactory> Database { get { return _database ?? (_database = CreateDatabase()); } }
    public int CardCount { get { return _cardSources.Length; } }

    public IEnumerator<Card> GetEnumerator()
    {
      return _previews.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public Card CreateCard(string name, Player controller, Game game)
    {
      ICardFactory cardFactory = GetCardFactory(name);
      return cardFactory.CreateCard(controller, game);
    }

    private ICardFactory GetCardFactory(string name)
    {
      ICardFactory cardFactory = Database
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

    private List<ICardFactory> CreateDatabase()
    {
      var database = new List<ICardFactory>();

      foreach (CardsSource cardSource in _cardSources)
      {
        database.AddRange(cardSource.GetCards());
      }

      return database;
    }

    public CardDatabase LoadPreviews()
    {
      _previews = Database
        .Select(x => x.CreateCardPreview())
        .ToDictionary(x => x.Name.ToLowerInvariant());

      return this;
    }
  }
}