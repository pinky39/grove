namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.CostRules;
  using Artifical.TargetingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Blaze : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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