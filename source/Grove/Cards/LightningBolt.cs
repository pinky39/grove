namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class LightningBolt : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lightning Bolt")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Lightning Bolt deals 3 damage to target creature or player.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = 3);
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.DealDamageSingleSelector(3);
          });
    }
  }
}