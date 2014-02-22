namespace Grove.Library
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class Intervene : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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

            p.TimingRule(new WhenTopSpellIsCounterable());
            p.TargetingRule(new EffectCounterspell());
          });
    }
  }
}