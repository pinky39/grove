namespace Grove.Core
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Details.Mana;
  using Ui;

  public class Deck : IEnumerable<string>
  {
    private static readonly Random Rnd = new Random();
    private readonly Dictionary<string, Card> _cards = new Dictionary<string, Card>();
    private readonly List<DeckRow> _rows = new List<DeckRow>();

    public Deck()
    {
      Name = "fantastic-new-deck.dec";
    }
    
    public Deck(string filename, IEnumerable<Card> previews)
    {
      Name = Path.GetFileName(filename);
      var deckFile = DeckFileReader.Read(filename);
      Rating = deckFile.Rating;
      Description = deckFile.Desctiption;

      foreach (var row in deckFile)
      {
        var card = previews
          .First(x => x.Name.Equals(row.CardName, StringComparison.CurrentCultureIgnoreCase));
        
        _cards.Add(card.Name.ToLowerInvariant(), card);
        _rows.Add(row);
      }
    }


    public IEnumerable<DeckRow> Creatures { get { return _rows.Where(x => GetCard(x.CardName).Is().Creature); } }

    public IEnumerable<DeckRow> Spells
    {
      get
      {
        return _rows.Where(x =>
          {
            var preview = GetCard(x.CardName);
            return !preview.Is().Creature && !preview.Is().Land;
          });
      }
    }

    public IEnumerable<DeckRow> Lands { get { return _rows.Where(x => GetCard(x.CardName).Is().Land); } }

    public int Rating { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Colors { get { return GetDeckColors(); } }
    public int CreaturesCount { get { return Creatures.Sum(x => x.Count); } }
    public int SpellsCount { get { return Spells.Sum(x => x.Count); } }
    public int LandsCount { get { return Lands.Sum(x => x.Count); } }
    public int CardCount { get { return _rows.Sum(x => x.Count); } }
    public string Name { get; set; }

    public IEnumerator<string> GetEnumerator()
    {
      foreach (var quantity in _rows)
      {
        for (var i = 0; i < quantity.Count; i++)
        {
          yield return quantity.CardName;
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    
    
    public void AddCard(Card card)
    {
      if (_cards.ContainsKey(card.Name.ToLowerInvariant()))
      {
        var row = GetCardRow(card.Name);
        row.Count++;
        return;
      }

      _cards.Add(card.Name.ToLowerInvariant(), card);
      _rows.Add(new DeckRow {CardName = card.Name, Count = 1});
    }

    private DeckRow GetCardRow(string cardName)
    {
      return _rows.FirstOrDefault(x => x.CardName.Equals(cardName, StringComparison.InvariantCultureIgnoreCase));
    }

    private List<string> GetDeckColors()
    {
      var colors = ManaColors.None;

      foreach (var card in _cards.Values)
      {
        colors = colors | card.Colors;
      }

      return ManaAmount.GetSymbolsFromColor(colors);
    }

    public Card GetCard(string cardName)
    {
      return _cards[cardName.ToLowerInvariant()];
    }

    public Card GetPreviewCard()
    {
      return _cards.Count == 0 ? null : _cards[_rows[Rnd.Next(0, _rows.Count)].CardName.ToLowerInvariant()];
    }

    public static Deck Dummy()
    {
      var deck = new Deck();
      deck._rows.Add(new DeckRow {CardName = "Uncastable", Count = 60});
      return deck;
    }

    public void RemoveCard(string name)
    {
      var row = GetCardRow(name);

      if (row == null)
        return;

      if (row.Count == 1)
      {
        _rows.Remove(row);
        _cards.Remove(name.ToLowerInvariant());
        return;
      }
      
      row.Count--;            
    }

    public static Deck GetRandomDeck(IEnumerable<Card> previews)
    {
      var decks = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec").ToList();
      return decks.Count > 0 ? new Deck(decks[Rnd.Next(0, decks.Count)], previews) : new Deck();
    }
    
  }
}