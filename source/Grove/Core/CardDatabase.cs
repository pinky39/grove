namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Dsl;

  public class CardDatabase
  {
    private readonly CardsSource[] _cardSources;
    private List<ICardFactory> _database;

    public CardDatabase(CardsSource[] cardSources)
    {
      _cardSources = cardSources;
    }

    private IEnumerable<ICardFactory> Database { get { return _database ?? (_database = CreateDatabase()); } }

    public int CardCount { get { return _cardSources.Length; } }

    public Card CreatePreview(string name)
    {
      var cardFactory = GetCardFactory(name);
      return cardFactory.CreateCardPreview();
    }

    public IEnumerable<Card> CreatePreviewForEveryCard()
    {
      return Database.Select(x => x.CreateCardPreview());
    }

    public Card CreateCard(string name, Player controller)
    {
      var cardFactory = GetCardFactory(name);
      return cardFactory.CreateCard(controller);
    }

    private ICardFactory GetCardFactory(string name)
    {
      var cardFactory = Database
        .Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        .FirstOrDefault();

      if (cardFactory == null)
        throw new InvalidOperationException(
          String.Format("Card with name '{0}' was not found in database.", name));
      return cardFactory;
    }

    public List<string> GetAvailableCardsNames()
    {
      return _database.Select(x => x.Name).OrderBy(x => x).ToList();
    }

    private List<ICardFactory> CreateDatabase()
    {
      var database = new List<ICardFactory>();

      foreach (var cardSource in _cardSources)
      {
        database.AddRange(cardSource.GetCards());
      }

      return database;
    }
  }
}