namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Shock : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Shock")
        .ManaCost("{R}")
        .Type("Instant")
        .Timing(Timings.TargetRemovalInstant())
        .Text("Shock deals 2 damage to target creature or player.")
        .Effect<DealDamageToTarget>(e => e.Amount = 2)
        .Targets(
          filter: TargetFilters.DealDamage(2),
          effect: C.Selector(Selectors.CreatureOrPlayer()));
    }
  }
}