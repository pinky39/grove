namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class SealOfFire : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Seal of Fire")
        .ManaCost("{R}")
        .Type("Enchantment")
        .Text("Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.")
        .FlavorText("I am the romancer, the passion that consumes the flesh.")
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.";
            p.Cost = new Sacrifice();
            p.Effect = () => new DealDamageToTargets(2);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new DealDamage(2));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}