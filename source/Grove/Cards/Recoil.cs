namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

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
        .Timing(Timings.InstantRemoval())
        .Category(EffectCategories.Bounce)
        .Effect<ReturnTargetPermanentToHand>((e, _) => e.Discard = 1)
        .Targets(C.Selector(
          validator: target => target.IsCard(),
          scorer: Core.Ai.TargetScores.OpponentStuffScoresMore()));
    }
  }
}