namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class BurstLightning : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Burst Lightning")
        .ManaCost("{R}")
        .KickerCost("{4}")
        .Type("Instant")
        .Text(
          "{Kicker} {4}{EOL}Burst Lightning deals 2 damage to target creature or player. If Burst Lightning was kicked, it deals 4 damage to that creature or player instead.")
        .Effect<DealDamageToTarget>((e, _) => e.Amount = 2)
        .Timing(Timings.InstantRemoval())
        .Targets(C.Selector(target => target.IsPlayer() || target.Is().Creature))
        .KickerEffect<DealDamageToTarget>((e, _) => e.Amount = 4)
        .KickerTargets(C.Selector(target => target.IsPlayer() || target.Is().Creature))
        .TargetFilter(TargetFilters.DealDamage(2))
        .KickerTargetsFilter(TargetFilters.DealDamage(4));
    }

    
  }  
}