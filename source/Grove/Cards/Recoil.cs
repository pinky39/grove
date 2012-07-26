namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Recoil : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Recoil")
        .ManaCost("{1}{U}{B}")
        .Type("Instant")
        .Text("Return target permanent to its owner's hand. Then that player discards a card.")
        .FlavorText("Anything sent into a plagued world is bound to come back infected.")
        .Timing(Timings.TargetRemovalInstant())
        .Category(EffectCategories.Bounce)
        .Effect<ReturnTargetCardToHand>(e => e.Discard = 1)
        .Targets(
          aiTargetSelector: AiTargetSelectors.Bounce(),
          effectValidator: C.Validator(Validators.Permanent()));
    }
  }
}