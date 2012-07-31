namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

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
        .Effect<DealDamageToTargets>(e => e.Amount = 3)
        .Targets(
          aiTargetSelector: TargetSelectorAi.DealDamageSingleSelector(3),
          effectValidator: C.Validator(Validators.CreatureOrPlayer()));
    }
  }
}