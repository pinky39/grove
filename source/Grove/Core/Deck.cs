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
    private readonly CardPreviews _cards;
    private static readonly Random Rnd = new Random();
    private readonly DeckFile _deckFile;
    
    public Deck(CardPreviews cards)
    {
      _cards = cards;
      _deckFile = new DeckFile();
    }

    public Deck(string filename, CardPreviews cards)
    {
      _cards = cards;
      Name = Path.GetFileName(filename);
      _deckFile = DeckFile.Read(filename);            
    }

    public IEnumerable<DeckRow> Creatures { get { return _deckFile.Where(x => GetCard(x.CardName).Is().Creature); } }
    
    public void Save()
    {
      var name = Name.ToLowerInvariant().Replace(".dec", String.Empty);     
      _deckFile.Write(MediaLibrary.GetDeckPath(name));
    }

    public IEnumerable<DeckRow> Spells
    {
      get
      {
        return _deckFile.Where(x =>
          {
            var preview = GetCard(x.CardName);
            return !preview.Is().Creature && !preview.Is().Land;
          });
      }
    }

    public IEnumerable<DeckRow> Lands { get { return _deckFile.Where(x => GetCard(x.CardName).Is().Land); } }

    public int Rating { get { return _deckFile.Rating; } set { _deckFile.Rating = value; } }
    public string Description { get { return _deckFile.Description; } set { _deckFile.Description = value; } }
    public IEnumerable<string> Colors { get { return GetDeckColors(); } }
    public int CreaturesCount { get { return Creatures.Sum(x => x.Count); } }
    public int SpellsCount { get { return Spells.Sum(x => x.Count); } }
    public int LandsCount { get { return Lands.Sum(x => x.Count); } }
    public int CardCount { get { return _deckFile.Sum(x => x.Count); } }
    public string Name { get; set; }

    public IEnumerator<string> GetEnumerator()
    {
      foreach (var quantity in _deckFile)
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
    
    public void AddCard(string name)
    {
      var row = GetCardRow(name);

      if (row == null)
      {
        _deckFile.AddRow(new DeckRow {CardName = name, Count = 1});
        return;
      }
                    
      row.Count++;            
    }

    private DeckRow GetCardRow(string cardName)
    {
      return _deckFile.FirstOrDefault(x => x.CardName.Equals(cardName, StringComparison.InvariantCultureIgnoreCase));
    }

    private List<string> GetDeckColors()
    {
      var colors = ManaColors.None;

      foreach (var deckRow in _deckFile)
      {
        colors = colors | _cards[deckRow.CardName].Colors;
      }            

      return ManaAmount.GetSymbolsFromColor(colors);
    }

    public Card GetCard(string cardName)
    {
      return _cards[cardName.ToLowerInvariant()];
    }

    public Card GetPreviewCard()
    {
      if (_deckFile.RowCount > 0)
      {
        return _cards[_deckFile.ElementAt(Rnd.Next(0, _deckFile.RowCount)).CardName];
      }

      return null;
    }

    public static Deck Dummy()
    {
      var deck = new Deck(null);
      deck._deckFile.AddRow(new DeckRow {CardName = "Uncastable", Count = 60});
      return deck;
    }

    public void RemoveCard(string name)
    {
      var row = GetCardRow(name);

      if (row == null)
        return;

      if (row.Count == 1)
      {
        _deckFile.RemoveRow(row);        
        return;
      }
      
      row.Count--;            
    }

    public static Deck GetRandomDeck(CardPreviews previews)
    {
      var decks = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec").ToList();
      return decks.Count > 0 ? new Deck(decks[Rnd.Next(0, decks.Count)], previews) : new Deck(previews);
    }
    
  }
}