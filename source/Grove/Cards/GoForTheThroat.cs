namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class GoForTheThroat : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Go for the Throat")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Destroy target nonartifact creature.")
        .FlavorText("Having flesh is increasingly a liability on Mirrodin.")
        .Effect<DestroyTargetPermanent>()
        .Timing(Timings.InstantRemoval())
        .Category(EffectCategories.Destruction)
        .Targets(C.Selector(
          validator: target => target.Is().Creature && !target.Is().Artifact,
          scorer: TargetScores.OpponentStuffScoresMore()));
    }
  }
}