namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;

  public class DeckRow
  {
    public CardInfo Card { get; set; }
    public int Count { get; set; }
    public Card CardData { get; set; }    

    public static IEnumerable<DeckRow> Group(IEnumerable<CardInfo> cards)
    {
      return cards
        .GroupBy(x => x.Name)
        .Select(x =>
        {
          var card = Cards.All[x.Key];
          return new DeckRow
          {
            Card = x.First(),
            Count = x.Count(),
            CardData = card,
          };
        })
        .OrderBy(x => x.CardData);            
    }
  }
}