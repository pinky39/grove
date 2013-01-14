namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;

  public class Sift : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sift")
        .ManaCost("{3}{U}")
        .Type("Sorcery")
        .Text("Draw three cards, then discard a card.")
        .FlavorText("Dwell longest on the thoughts that shine brightest.")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<DrawCards>(e =>
              {
                e.DrawCount = 3;
                e.DiscardCount = 1;
              });
          });
    }
  }
}