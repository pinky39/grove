namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class SwordsToPlowshares : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Swords to Plowshares")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Exile target creature. Its controller gains life equal to its power.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Category = EffectCategories.Exile;
            p.Effect = Effect<ExileTargets>(e => e.ControllerGainsLifeEqualToToughness = true);
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.Exile();
          });
    }
  }
}