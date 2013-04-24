namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Rewind : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rewind")
        .ManaCost("{2}{U}{U}")
        .Type("Instant")
        .Text("Counter target spell. Untap up to four lands.")
        .FlavorText("Time flows like a river. In Tolaria we practice the art of building dams")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new CounterTargetSpell(),
              new UntapSelectedPermanents(
                minCount: 0,
                maxCount: 4,
                validator: c => c.Is().Land,
                text: "Select lands to untap."
                ));

            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
            
            p.TargetingRule(new Ai.TargetingRules.Counterspell());
            p.TimingRule(new Ai.TimingRules.Counterspell());
          });
    }
  }
}