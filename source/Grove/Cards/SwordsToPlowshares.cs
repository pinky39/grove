namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class SwordsToPlowshares : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Swords to Plowshares")
        .ManaCost("{W}")
        .Type("Instant")
        .Timing(Timings.TargetRemovalInstant())
        .Category(EffectCategories.Exile)
        .Text("Exile target creature. Its controller gains life equal to its power.")
        .Effect<ExileTargetPermanent>((e, _) => e.ControllerGainsLifeEqualToToughness = true)
        .Targets(
          filter: TargetFilters.Exile(),
          selectors: C.Selector(
            Selectors.Creature()));
    }
  }
}