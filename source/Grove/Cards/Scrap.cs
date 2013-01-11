namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Scrap : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Scrap")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Destroy target artifact.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyTargetPermanents>();
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Artifact), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.OrderByScore();
          });
    }
  }
}