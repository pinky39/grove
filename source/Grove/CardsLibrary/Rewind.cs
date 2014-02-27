namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Rewind : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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

            p.TargetingRule(new EffectCounterspell());
            p.TimingRule(new WhenTopSpellIsCounterable());
          });
    }
  }
}