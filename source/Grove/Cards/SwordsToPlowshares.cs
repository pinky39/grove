namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class SwordsToPlowshares : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Swords to Plowshares")
        .ManaCost("{W}")
        .Type("Instant")
        .Timing(Timings.InstantRemoval)
        .Text("Exile target creature. Its controller gains life equal to its power.")
        .Effect<ExileTargetPermanent>((e, _) => e.ControllerGainsLifeEqualToToughness = true)
        .Target(C.Selector(
          validator: target => target.Is().Creature,
          scorer: TargetScores.OpponentStuffScoresMore()));
    }
  }
}