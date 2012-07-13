namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

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
        .Timing(Timings.FirstMain())
        .Effect<TargetPlayerTakesExtraTurns>()
        .Targets(
          filter: TargetFilters.Controller(),
          effect: C.Selector(Selectors.Player()));
    }
  }
}