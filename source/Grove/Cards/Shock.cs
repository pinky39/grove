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
        .Text("Shock deals 2 damage to target creature or player.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = 2);
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.DealDamageSingleSelector(2);
          });

    }
  }
}