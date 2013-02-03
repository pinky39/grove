namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;

  public class Catalog : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Catalog")
        .ManaCost("{2}{U}")
        .Type("Instant")
        .Text("Draw two cards, then discard a card.")
        .FlavorText("'Without order comes errors, and errors kill on Tolaria.'{EOL}—Barrin, master wizard")
        .Cast(p =>
          {
            p.Timing = Timings.EndOfTurn();
            p.Effect = Effect<DrawCards>(e =>
              {
                e.Count = 2;
                e.DiscardCount = 1;
              });
          });
    }
  }
}