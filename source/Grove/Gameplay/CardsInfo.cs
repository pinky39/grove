namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using System.Linq;

  public class CardsInfo
  {
    private readonly Dictionary<string, Card> _cards = new Dictionary<string, Card>();

    public CardsInfo(CardsDatabase cardsDatabase)
    {
      _cards = cardsDatabase.CreateAll()
        .ToDictionary(x => x.Name.ToLowerInvariant(), x => x);
    }

    public int Count { get { return _cards.Count; } }
    public Card this[string name] { get { return _cards[name.ToLowerInvariant()]; } }

    public List<string> GetCardNames()
    {
      return _cards.Values.Select(x => x.Name).OrderBy(x => x).ToList();
    }
  }
}