namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.CostRules;
  using Core.Ai.TargetingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

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