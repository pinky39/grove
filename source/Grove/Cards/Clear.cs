namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Clear : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Clear")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy target enchantment.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyTargetPermanents>();
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Enchantment), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.OrderByScore();
          });
    }
  }
}