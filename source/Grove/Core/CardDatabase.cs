namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class CardDatabase
  {
    private readonly CardsSource[] _cardSources;    
    private List<ICardFactory> _database;    

    public CardDatabase(CardsSource[] cardSources)
    {
      _cardSources = cardSources;      
    }

    private IEnumerable<ICardFactory> Database
    {
      get { return _database ?? (_database = CreateDatabase()); }
    }

    public int CardCount { get { return _cardSources.Length; } }

    public Card CreateCard(string name, Player controller)
    {
      var cardFactory = Database
        .Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        .FirstOrDefault();

      if (cardFactory == null)
        throw new InvalidOperationException(
          String.Format("Card with name '{0}' was not found in database.", name));

      return cardFactory.CreateCard(controller);
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