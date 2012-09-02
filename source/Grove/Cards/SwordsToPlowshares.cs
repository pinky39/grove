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
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Exile)
        .Text("Exile target creature. Its controller gains life equal to its power.")
        .Effect<ExileTargets>(e => e.ControllerGainsLifeEqualToToughness = true)
        .Targets(
          selectorAi: TargetSelectorAi.Exile(),
          effectValidator: C.Validator(
            Validators.Creature()));
    }
  }
}