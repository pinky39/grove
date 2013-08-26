namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Undermine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Undermine")
        .ManaCost("{U}{U}{B}")
        .Type("Instant")
        .Text("Counter target spell. Its controller loses 3 life.")
        .FlavorText("'Which would you like first, the insult or the injury?'")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell(controllerLifeloss: 3);
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
            p.TimingRule(new Artifical.TimingRules.WhenTopSpellIsCounterable());
            p.TargetingRule(new Artifical.TargetingRules.EffectCounterspell());
          });
    }
  }
}