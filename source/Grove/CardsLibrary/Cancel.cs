namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Cancel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cancel")
        .ManaCost("{1}{U}{U}")
        .Type("Instant")
        .Text("Counter target spell.")
        .FlavorText("\"Even the greatest inferno begins as a spark. And anyone can snuff out a spark.\"{EOL}—Chanyi, mistfire sage")
        .Cast(p =>
        {
          p.Effect = () => new CounterTargetSpell();
          p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

          p.TargetingRule(new EffectCounterspell());
          p.TimingRule(new WhenTopSpellIsCounterable());
        });
    }
  }
}
