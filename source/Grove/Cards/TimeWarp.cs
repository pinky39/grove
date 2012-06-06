namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Effects;

  public class TimeWarp : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Time Warp")
        .ManaCost("{3}{U}{U}")
        .Type("Sorcery")
        .Text("Target player takes an extra turn after this one.")
        .FlavorText("Just when you thought you'd survived the first wave.")
        .Timing(Timings.Steps(Step.SecondMain))
        .Effect<TargetPlayerTakesExtraTurns>()
        .Target(C.Selector(
          validator: target => target.IsPlayer(),
          scorer: Core.Ai.TargetScores.YourStuffScoresMore()));
    }
  }
}