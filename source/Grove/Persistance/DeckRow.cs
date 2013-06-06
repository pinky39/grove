namespace Grove.Persistance
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;

  public class DeckRow
  {
    public CardInfo Card { get; set; }
    public int Count { get; set; }

    public static IEnumerable<DeckRow> Group(IEnumerable<CardInfo> cards)
    {
      return cards
        .GroupBy(x => x.Name)
        .Select(x => new DeckRow
          {
            Card = x.First(),
            Count = x.Count()
          });
    }
  }
}