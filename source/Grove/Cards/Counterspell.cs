namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Counterspell : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Counterspell")
        .ManaCost("{U}{U}")
        .Type("Instant")
        .Text("Counter target spell.")
        .FlavorText("Your attack has been rendered harmless. It is, however, quite pretty.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell();
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

            p.TargetingRule(new Artifical.TargetingRules.Counterspell());
            p.TimingRule(new Artifical.TimingRules.Counterspell());
          });
    }
  }
}