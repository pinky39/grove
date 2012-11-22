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
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Exile)
        .Text("Exile target creature. Its controller gains life equal to its power.")
        .Effect<ExileTargets>(e => e.ControllerGainsLifeEqualToToughness = true)
        .Targets(
          TargetSelectorAi.Exile(),
          TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield()));
    }
  }
}