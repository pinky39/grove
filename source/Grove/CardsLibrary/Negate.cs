namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Negate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Negate")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Counter target noncreature spell.")
        .FlavorText(
          "Masters of the arcane savor a delicious irony. Their study of deep and complex arcana leads to such a simple end: the ability to say merely yes or no.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell();

            p.TargetSelector.AddEffect(t => t
              .Is.CounterableSpell(e => !e.Source.OwningCard.Is().Creature)
              .On.Stack());

            p.TargetingRule(new EffectCounterspell());
            p.TimingRule(new WhenTopSpellIsCounterable());
          });
    }
  }
}