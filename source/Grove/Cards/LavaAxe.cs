namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class LavaAxe :CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lava Axe")
        .ManaCost("{4}{R}")
        .Type("Sorcery")
        .Text("Lava Axe deals 5 damage to target player.")
        .FlavorText("'Catch'")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(5);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectDealDamage(5));
            p.TimingRule(new TargetRemovalTimingRule());
          });
    }
  }
}