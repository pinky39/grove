namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Recoil : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Recoil")
        .ManaCost("{1}{U}{B}")
        .Type("Instant")
        .Text("Return target permanent to its owner's hand. Then that player discards a card.")
        .FlavorText("Anything sent into a plagued world is bound to come back infected.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Category = EffectCategories.Bounce;
            p.Effect = Effect<ReturnToHand>(e => { e.Discard = 1; });
            p.EffectTargets = L(Target(Validators.Card(), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.Bounce();
          });
    }
  }
}