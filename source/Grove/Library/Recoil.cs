namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class Recoil : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Recoil")
        .ManaCost("{1}{U}{B}")
        .Type("Instant")
        .Text("Return target permanent to its owner's hand. Then that player discards a card.")
        .FlavorText("Anything sent into a plagued world is bound to come back infected.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnToHand(discard: 1);
            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());

            p.TargetingRule(new EffectBounce());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Bounce));
          });
    }
  }
}