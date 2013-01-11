namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Casting;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class BeaconOfDestruction : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Beacon of Destruction")
        .ManaCost("{3}{R}{R}")
        .Type("Instant")
        .Text(
          "Beacon of Destruction deals 5 damage to target creature or player. Shuffle Beacon of Destruction into its owner's library.")
        .FlavorText("The Great Furnace's blessing is a spectacular sight, but the best view comes at a high cost.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Rule = Rule<Instant>(r => r.AfterResolvePutToZone = card => card.ShuffleIntoLibrary());
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = 5);
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.DealDamageSingleSelector(5);
          });
    }
  }
}