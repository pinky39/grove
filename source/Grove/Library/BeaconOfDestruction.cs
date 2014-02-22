namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class BeaconOfDestruction : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Rule = new Instant(c => c.ShuffleIntoLibrary());
            p.Effect = () => new DealDamageToTargets(5);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TargetingRule(new EffectDealDamage(5));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
          });
    }
  }
}