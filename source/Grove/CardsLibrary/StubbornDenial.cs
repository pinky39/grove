namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class StubbornDenial : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stubborn Denial")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Counter target noncreature spell unless its controller pays {1}.{EOL}{I}Ferocious{/I} — If you control a creature with power 4 or greater, counter that spell instead.")
        .FlavorText("The Temur have no patience for subtlety.")
        .Cast(p =>
        {
          p.Effect = () => new FerociousEffect(
            normalEffects: new Effect[]
            {
              new CounterTargetSpell(doNotCounterCost: 1),
            },
            ferociousEffects: new Effect[]
            {
              new CounterTargetSpell(),
            });

          p.TargetSelector.AddEffect(trg => trg
            .Is.CounterableSpell(e => !e.Source.OwningCard.Is().Creature)
            .On.Stack());

          p.TimingRule(new WhenTopSpellIsCounterable(1));
          p.TargetingRule(new EffectCounterspell());
        });
    }
  }
}
