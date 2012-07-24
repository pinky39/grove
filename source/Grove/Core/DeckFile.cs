namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;

  public class DeckFile : IEnumerable<DeckRow>
  {
    public string Desctiption { get; private set; }
    public int Rating { get; private set; }

    private readonly List<DeckRow> _rows = new List<DeckRow>();

    public DeckFile(IEnumerable<DeckRow> rows, string desctiption, int rating)
    {
      Desctiption = desctiption;
      Rating = rating;

      _rows.AddRange(rows);
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