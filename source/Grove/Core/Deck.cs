namespace Grove.Core
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Mana;
  using Ui;

  public class Deck : IEnumerable<Card>
  {
    private readonly CardDatabase _cardDatabase;
    private readonly List<Card> _cards = new List<Card>();

    public Deck(CardDatabase cardDatabase)
    {
      _cardDatabase = cardDatabase;
    }

    public Deck(IEnumerable<string> cards, CardDatabase cardDatabase, string name = null, int rating = 3,
      string description = null)
    {
      _cards = cards.Select(x => cardDatabase[x]).ToList();
      _cardDatabase = cardDatabase;

      Name = name;
      Rating = rating;
      Description = description;
    }

    public IEnumerable<Card> Creatures { get { return _cards.Where(x => x.Is().Creature); } }
    public IEnumerable<Card> Spells { get { return _cards.Where(x => !x.Is().Creature && !x.Is().Land); } }
    public IEnumerable<Card> Lands { get { return _cards.Where(x => x.Is().Land); } }
    public IEnumerable<ManaColor> Colors { get { return GetDeckManaColors(); } }

    public int Rating { get; set; }
    public string Description { get; set; }

    public string Name { get; set; }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    public void Save()
    {
      var name = Name.ToLowerInvariant()
        .Replace(".dec", String.Empty);

      new DeckReaderWriter().Write(this, MediaLibrary.GetDeckPath(name));
    }

    public void AddCard(string name)
    {
      _cards.Add(_cardDatabase[name]);
    }

    private List<ManaColor> GetDeckManaColors()
    {
      var dictionary = new Dictionary<ManaColor, bool>
        {
          {ManaColor.White, false},
          {ManaColor.Blue, false},
          {ManaColor.Black, false},
          {ManaColor.Red, false},
          {ManaColor.Green, false},
        };

      foreach (var card in _cards)
      {
        foreach (var singleColorAmount in card.ManaCost)
        {
          dictionary[singleColorAmount.Color] = true;
        }
      }

      return dictionary
        .Where(x => x.Value)
        .Select(x => x.Key)
        .ToList();
    }

    public void RemoveCard(string name)
    {
      var card = _cards.FirstOrDefault(x => x.Name == name);

      if (card != null)
      {
        _cards.Remove(card);
      }
    }

    public static implicit operator List<string>(Deck deck)
    {
      return deck.Select(x => x.Name).ToList();
    }
  }
}