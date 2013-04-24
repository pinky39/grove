namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.CostRules;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class HeatRay : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Heat Ray")
        .ManaCost("{R}").HasXInCost()
        .Type("Instant")
        .Text("Heat Ray deals X damage to target creature.")
        .FlavorText("It's not known whether the Thran built the device to forge their wonders or to defend them.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(Value.PlusX);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new DealDamage());
            p.CostRule(new TargetsLifepoints());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}