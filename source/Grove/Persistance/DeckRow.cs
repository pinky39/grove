namespace Grove.Persistance
{
  using System.Collections.Generic;
  using System.Linq;

  public class DeckRow
  {
    public string CardName { get; set; }
    public int Count { get; set; }

    public static IEnumerable<DeckRow> Group(IEnumerable<string> cardNames)
    {
      return cardNames
        .GroupBy(x => x)
        .Select(x => new DeckRow
          {
            CardName = x.Key,
            Count = x.Count()
          });
    }
  }
}