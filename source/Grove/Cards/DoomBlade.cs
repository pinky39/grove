namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Effects;

  public class DoomBlade : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Doom blade")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Destroy target nonblack creature.")
        .FlavorText("The void is without substance but cuts like steel.")
        .Effect<DestroyTargetPermanent>()
        .Timing(Timings.InstantRemoval)
        .Category(EffectCategories.Destruction)
        .Target(C.Selector(
          validator: target => target.Is().Creature && !target.HasColor(ManaColors.Black),
          scorer: Core.Ai.TargetScores.OpponentStuffScoresMore()));
    }
  }
}