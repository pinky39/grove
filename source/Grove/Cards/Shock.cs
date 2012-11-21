namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Shock : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Shock")
        .ManaCost("{R}")
        .Type("Instant")
        .Timing(Timings.InstantRemovalTarget())
        .Text("Shock deals 2 damage to target creature or player.")
        .Effect<DealDamageToTargets>(e => e.Amount = 2)
        .Targets(
          selectorAi: TargetSelectorAi.DealDamageSingleSelector(2),
          effectValidators: TargetValidator(TargetIs.CreatureOrPlayer()));
    }
  }
}