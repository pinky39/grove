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
    public IEnumerable<string> Colors { get { return GetDeckColors(); } }

    public int Rating { get; set; }
    public string Description { get; set; }

    public string Name { get; set; }
    public IEnumerable<string> CardNames { get { return _cards.Select(x => x.Name); } }

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
      string name = Name.ToLowerInvariant()
        .Replace(".dec", String.Empty);

      new DeckReaderWriter().Write(this, MediaLibrary.GetDeckPath(name));
    }

    public void AddCard(string name)
    {
      _cards.Add(_cardDatabase[name]);
    }

    private List<string> GetDeckColors()
    {
      ManaColors colors = ManaColors.None;

      foreach (Card card in _cards)
      {
        colors = colors | card.Colors;
      }

      return ManaAmount.GetSymbolsFromColor(colors);
    }

    public void RemoveCard(string name)
    {
      Card card = _cards.FirstOrDefault(x => x.Name == name);

      if (card != null)
      {
        _cards.Remove(card);
      }
    }
  }
}