namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;

  public class Counterspell : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Counterspell")
        .ManaCost("{U}{U}")
        .Type("Instant")
        .Text("Counter target spell.")
        .FlavorText("'Your attack has been rendered harmless. It is, however, quite pretty.'{EOL}—Saprazzan vizier")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell();
            p.TargetSelector.AddEffect(trg => trg.Is.Counterable().On.Stack());
            p.TimingRule(new Core.Ai.TimingRules.Counterspell());
            p.TargetingRule(new Core.Ai.TargetingRules.Counterspell());
          });
    }
  }
}