namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Ai.CostRules;
  using Ai.TargetingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class Blaze : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Blaze")
        .ManaCost("{R}").HasXInCost()
        .Type("Sorcery")
        .Text("Blaze deals X damage to target creature or player.")
        .FlavorText("Fire never dies alone.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(Value.PlusX);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
                        
            p.TargetingRule(new DealDamage());
            p.CostRule(new TargetsLifepoints());
          });
    }
  }
}