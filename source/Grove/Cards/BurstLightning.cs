namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class BurstLightning : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Burst Lightning")
        .ManaCost("{R}")
        .KickerCost("{4}")
        .Type("Instant")
        .Text(
          "{Kicker} {4}{EOL}Burst Lightning deals 2 damage to target creature or player. If Burst Lightning was kicked, it deals 4 damage to that creature or player instead.")
        .Timing(Timings.InstantRemovalTarget())
        .Effect<DealDamageToTargets>(p => p.Amount = 2)
        .Targets(
          selectorAi: TargetSelectorAi.DealDamageSingleSelector(2),
          effectValidator: TargetValidator(
            TargetIs.CreatureOrPlayer(),
            ZoneIs.Battlefield()))
        .KickerEffect<DealDamageToTargets>(p => p.Amount = 4)
        .KickerTargets(
          aiTargetSelector: TargetSelectorAi.DealDamageSingleSelector(4),
          effectValidators: TargetValidator(
            TargetIs.CreatureOrPlayer(),
            ZoneIs.Battlefield()));
    }
  }
}