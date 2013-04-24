namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class LightningBolt : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Lightning Bolt")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Lightning Bolt deals 3 damage to target creature or player.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new DealDamage(3));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}