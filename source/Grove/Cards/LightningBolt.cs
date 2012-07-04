namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class LightningBolt : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Lightning Bolt")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Lightning Bolt deals 3 damage to target creature or player.")
        .Timing(Timings.TargetRemovalInstant())
        .Effect<DealDamageToTarget>((e, _) => e.Amount = 3)
        .Targets(
          filter: TargetFilters.DealDamage(3),
          selectors: C.Selector(Selectors.CreatureOrPlayer()));
    }
  }
}