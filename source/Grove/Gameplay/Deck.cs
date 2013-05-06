namespace Grove.Gameplay
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using ManaHandling;
  using Persistance;
  using UserInterface;

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
    public IManaAmount Colors { get { return GetDeckManaColors(); } }

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

    private IManaAmount GetDeckManaColors()
    {
      var dictionary = new Dictionary<ManaColor, bool>();

      foreach (var card in _cards)
      {
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
      return Enumerable.ToList<string>(deck.Select(x => x.Name));
    }
  }
}