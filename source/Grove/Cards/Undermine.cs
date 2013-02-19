namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;

  public class Undermine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TimingRule(new Core.Ai.TimingRules.Counterspell());
            p.TargetingRule(new Core.Ai.TargetingRules.Counterspell());
          });
    }
  }
}