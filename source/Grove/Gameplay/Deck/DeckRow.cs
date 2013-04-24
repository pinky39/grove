namespace Grove.Gameplay.Deck
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Card;

  public class DeckRow : IEnumerable<string>
  {
    public string CardName { get; set; }
    public int Count { get; set; }

    public IEnumerator<string> GetEnumerator()
    {
      return Enumerable.Repeat(CardName, Count).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  public static class RowConversions
  {
    public static IEnumerable<DeckRow> AsRows(this IEnumerable<Card> cards)
    {
      return cards.Select(x => x.Name).AsRows();
    }

    public static IEnumerable<DeckRow> AsRows(this IEnumerable<string> cardNames)
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