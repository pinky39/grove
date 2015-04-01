namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class StatuteOfDenial : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Statute of Denial")
        .ManaCost("{2}{U}{U}")
        .Type("Instant")
        .Text("Counter target spell. If you control a blue creature, draw a card, then discard a card.")
        .FlavorText(
          "\"Pyrotechnic activity of any sort is strictly prohibited. It is irrelevant that today is a holiday.\"")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new CounterTargetSpell(),
              new DrawCards(1, discardCount: 1)
                {
                  ShouldResolve = ctx => ctx.You.Battlefield.Any(x => x.HasColor(CardColor.Blue) && x.Is().Creature)
                });

            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

            p.TargetingRule(new EffectCounterspell());
            p.TimingRule(new WhenTopSpellIsCounterable());
          });
    }
  }
}