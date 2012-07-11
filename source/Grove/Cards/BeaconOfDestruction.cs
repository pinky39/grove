namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class BeaconOfDestruction : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Beacon of Destruction")
        .ManaCost("{3}{R}{R}")
        .Type("Instant")
        .Text(
          "Beacon of Destruction deals 5 damage to target creature or player. Shuffle Beacon of Destruction into its owner's library.")
        .FlavorText("The Great Furnace's blessing is a spectacular sight, but the best view comes at a high cost.")
        .Timing(Timings.TargetRemovalInstant())
        .AfterResolvePutToZone(Zone.Library)
        .Effect<DealDamageToTarget>((e, _) => e.SetAmount(5))
        .Targets(
          filter: TargetFilters.DealDamage(5),
          selectors: C.Selector(Selectors.CreatureOrPlayer()));
    }
  }
}