namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class BurstLightning : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Burst Lightning")
        .ManaCost("{R}")
        .Type("Instant")
        .Text(
          "{Kicker} {4}{EOL}Burst Lightning deals 2 damage to target creature or player. If Burst Lightning was kicked, it deals 4 damage to that creature or player instead.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = 2);
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.DealDamageSingleSelector(2);
          })
        .Cast(p =>
          {
            p.Description = p.KickerDescription;
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = 4);
            p.Cost = Cost<PayMana>(c => c.Amount = "{4}{R}".ParseMana());
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.DealDamageSingleSelector(4);
          });
    }
  }
}