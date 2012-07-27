namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

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
        .Timing(Timings.TargetRemovalInstant())
        .Effect<DealDamageToTargets>(p => p.Amount = 2)
        .Targets(
          aiTargetSelector: AiTargetSelectors.DealDamageSingleSelector(2),
          effectValidator: C.Validator(Validators.CreatureOrPlayer()))
        .KickerEffect<DealDamageToTargets>(p => p.Amount = 4)
        .KickerTargets(
          aiTargetSelector: AiTargetSelectors.DealDamageSingleSelector(4),
          effectValidators: C.Validator(Validators.CreatureOrPlayer()));
    }
  }
}