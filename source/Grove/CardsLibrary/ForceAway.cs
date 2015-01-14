namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class ForceAway : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Force Away")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text(
          "Return target creature to its owner's hand.{EOL}{I}Ferocious{/I} — If you control a creature with power 4 or greater, you may draw a card. If you do, discard a card.")
        .FlavorText("Where an enemy once rode, not even a whisper remains.")
        .Cast(p =>
        {
          p.Effect = () => new FerociousEffect(
            normalEffects: new Effect[]
            {
              new ReturnToHand(),
            },
            ferociousEffects: new Effect[]
            {
              new ReturnToHand(),
              new DrawCards(1, discardCount: 1),
            });

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectBounce());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.CreaturesOnly));
        });
    }
  }
}
