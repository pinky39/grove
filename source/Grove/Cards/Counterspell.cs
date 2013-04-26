namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Counterspell : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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

            p.TargetingRule(new Ai.TargetingRules.Counterspell());
            p.TimingRule(new Ai.TimingRules.Counterspell());
          });
    }
  }
}