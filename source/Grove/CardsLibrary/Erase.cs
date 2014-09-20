namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Erase : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Erase")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Exile target enchantment.")
        .FlavorText("Perception is more pleasing than truth.")
        .Cast(p =>
          {
            p.Effect = () => new ExileTargets();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());
            p.TargetingRule(new EffectExileBattlefield());
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.Exile));
          });
    }
  }
}