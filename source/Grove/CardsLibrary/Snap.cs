namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Snap : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Snap")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Return target creature to its owner's hand. Untap up to two lands.")
        .FlavorText("Good riddance.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ReturnToHand(),
              new UntapSelectedPermanents(
                minCount: 0,
                maxCount: 2,
                validator: c => c.Is().Land,
                text: "Select lands to untap."
                )
              );

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectBounce());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.CreaturesOnly));
          });
    }
  }
}