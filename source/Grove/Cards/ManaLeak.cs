namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;

  public class ManaLeak : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Mana Leak")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Counter target spell unless its controller pays {3}.")
        .FlavorText("The fatal flaw in every plan is the assumption that you know more than your enemy.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell(doNotCounterCost: 3);
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
            p.TimingRule(new Core.Ai.TimingRules.Counterspell(3));
            p.TargetingRule(new Core.Ai.TargetingRules.Counterspell());
          });
    }
  }
}