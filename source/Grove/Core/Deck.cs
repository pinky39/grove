namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Details.Mana;

  public class Deck : IEnumerable<string>
  {
    private readonly List<string> _colors;
    private readonly Dictionary<string, Card> _previews = new Dictionary<string, Card>();
    private readonly List<DeckRow> _rows = new List<DeckRow>();

    public Deck(string filename, CardDatabase cardDatabase)
    {
      Name = Path.GetFileName(filename);

      var deckFile = DeckFileReader.Read(filename);

      Rating = deckFile.Rating;
      Description = deckFile.Desctiption;

      foreach (var row in deckFile)
      {
        var preview = cardDatabase.CreatePreview(row.CardName);
        _previews.Add(preview.Name.ToLowerInvariant(), preview);
        _rows.Add(row);
      }

      _colors = GetDeckColors();
    }

    public IEnumerable<DeckRow> Creatures { get { return _rows.Where(x => GetPreview(x.CardName).Is().Creature); } }

    public IEnumerable<DeckRow> Spells
    {
      get
      {
        return _rows.Where(x =>
          {
            var preview = GetPreview(x.CardName);
            return !preview.Is().Creature && !preview.Is().Land;
          });
      }
    }

    public IEnumerable<DeckRow> Lands { get { return _rows.Where(x => GetPreview(x.CardName).Is().Land); } }

    public int Rating { get; private set; }
    public string Description { get; private set; }
    public IEnumerable<string> Colors { get { return _colors; } }
    public int CreaturesCount { get { return Creatures.Sum(x => x.Count); } }
    public int SpellsCount { get { return Spells.Sum(x => x.Count); } }
    public int LandsCount { get { return Lands.Sum(x => x.Count); } }
    public int CardCount { get { return _rows.Sum(x => x.Count); } }

    public string Name { get; private set; }

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

    private List<string> GetDeckColors()
    {
      var colors = ManaColors.None;

      foreach (var card in _previews.Values)
      {
        colors = colors | card.Colors;
      }

      return ManaAmount.GetSymbolsFromColor(colors);
    }

    public Card GetPreview(string cardName)
    {
      return _previews[cardName.ToLowerInvariant()];
    }

    public Card GetPreview()
    {
      return _previews[_rows[0].CardName.ToLowerInvariant()];
    }
  }
}