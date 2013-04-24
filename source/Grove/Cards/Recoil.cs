namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Recoil : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Recoil")
        .ManaCost("{1}{U}{B}")
        .Type("Instant")
        .Text("Return target permanent to its owner's hand. Then that player discards a card.")
        .FlavorText("Anything sent into a plagued world is bound to come back infected.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnToHand(discard: 1) {Category = EffectCategories.Bounce};
            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());

            p.TargetingRule(new Bounce());
            p.TimingRule(new TargetRemoval());            
          });
    }
  }
}