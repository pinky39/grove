namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class TimeWarp : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Time Warp")
        .ManaCost("{3}{U}{U}")
        .Type("Sorcery")
        .Text("Target player takes an extra turn after this one.")
        .FlavorText("Just when you thought you'd survived the first wave.")
        .Timing(Timings.FirstMain())
        .Effect<TargetPlayerTakesExtraTurns>()
        .Targets(
          selectorAi: TargetSelectorAi.Controller(),
          effectValidator: TargetValidator(TargetIs.Player(), ZoneIs.None()));
    }
  }
}