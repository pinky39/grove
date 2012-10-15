namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;

  public class DeckFile : IEnumerable<DeckRow>
  {
    private readonly List<DeckRow> _rows = new List<DeckRow>();

    public DeckFile(IEnumerable<DeckRow> rows, string desctiption, int rating)
    {
      Desctiption = desctiption;
      Rating = rating;

      _rows.AddRange(rows);
    }

    public string Desctiption { get; private set; }
    public int Rating { get; private set; }

    public IEnumerable<string> AllCards
    {
      get
      {
        foreach (var row in _rows)
        {
          for (int i = 0; i < row.Count; i++)
          {
            yield return row.CardName;
          }
        }
      }
    }

    public IEnumerator<DeckRow> GetEnumerator()
    {
      return _rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}