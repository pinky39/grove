namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class BatheInDragonfire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bathe in Dragonfire")
        .ManaCost("{2}{R}")
        .Type("Sorcery")
        .Text("Bathe in Dragonfire deals 4 damage to target creature.")
        .FlavorText("The scent of cooked flesh lingers in the charred landscape of Tarkir.")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToTargets(4);
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectDealDamage(4));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
