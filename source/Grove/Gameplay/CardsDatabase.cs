namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Infrastructure;
  using Misc;

  [Serializable]
  public class CardsDatabase : ISerializable
  {    
    private readonly List<CardFactory> _factories;

    public CardsDatabase(IEnumerable<CardsSource> cardSources)
    {
      _factories = GetFactories(cardSources);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.SetType(typeof (SingletonSerializationHelper<CardsDatabase>));
    }

    public Card CreateCard(string name)
    {
      var cardFactory = GetFactory(name);
      return cardFactory.CreateCard();
    }

    public List<Card> CreateAll()
    {
      return _factories
        .Select(x => x.CreateCard())
        .Where(x => !x.Is("uncastable"))
        .ToList();
    }

    private CardFactory GetFactory(string name)
    {
      var cardFactory = _factories
        .Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        .FirstOrDefault();

      if (cardFactory == null)
        throw new InvalidOperationException(
          String.Format("Card with name '{0}' was not found in database.", name));
      return cardFactory;
    }

    private static List<CardFactory> GetFactories(IEnumerable<CardsSource> cardsSources)
    {
      var factories = new List<CardFactory>();
      foreach (var cardSource in cardsSources)
      {
        factories.AddRange(cardSource.GetCards());
      }
      return factories;
    }
  }
}