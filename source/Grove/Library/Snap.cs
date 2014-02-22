namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

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