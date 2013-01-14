namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;

  public class Hush : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hush")
        .ManaCost("{3}{G}")
        .Type("Sorcery")
        .Text("Destroy all enchantments.{EOL}{Cycling} {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyAllPermanents>(e => e.Filter = (self, card) => card.Is().Enchantment);
          });
    }
  }
}