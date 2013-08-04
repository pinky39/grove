namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class Intervene : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Intervene")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Counter target spell that targets a creature.")
        .FlavorText("At first I simply observed. But I found that without investment in others, life serves no purpose.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell();
            
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell(e => 
              e.Targets.Effect.Any(x => x.IsCard() && x.Card().Is().Creature)).On.Stack());            

            p.TimingRule(new Artifical.TimingRules.Counterspell());
            p.TargetingRule(new Artifical.TargetingRules.Counterspell());
          });
    }
  }
}