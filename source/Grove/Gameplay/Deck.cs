namespace Grove.Gameplay
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using ManaHandling;

  public class Deck : IEnumerable<string>
  {
    private readonly List<string> _cardNames = new List<string>();
    private readonly CardsInfo _cardsInfo;

    public Deck(CardsInfo cardsInfo)
    {
      _cardsInfo = cardsInfo;
    }

    public IEnumerable<string> Creatures { get { return _cardNames.Where(x => _cardsInfo[x].Is().Creature); } }
    public IEnumerable<string> Spells { get { return _cardNames.Where(x => !_cardsInfo[x].Is().Creature && !_cardsInfo[x].Is().Land); } }
    public IEnumerable<string> Lands { get { return _cardNames.Where(x => _cardsInfo[x].Is().Land); } }

    public IManaAmount Colors
    {
      get
      {
        var dictionary = new Dictionary<ManaColor, bool>();

        foreach (var cardName in _cardNames)
        {
          var card = _cardsInfo[cardName];

          if (card.ManaCost == null)
            continue;

          foreach (var singleColorAmount in card.ManaCost)
          {
            dictionary[singleColorAmount.Color] = true;
          }
        }

        return new MultiColorManaAmount(dictionary
          .Where(x => x.Value)
          .Where(x => x.Key != ManaColor.Colorless)
          .ToDictionary(x => x.Key, x => 1));
      }
    }

    public int Rating { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }    

    public IEnumerator<string> GetEnumerator()
    {
      return _cardNames.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void AddCard(string name, int count = 1)
    {
      for (var i = 0; i < count; i++)
      {
        _cardNames.Add(name);
      }
    }

    public bool RemoveCard(string name)
    {
      return _cardNames.Remove(name);
    }
  }
}