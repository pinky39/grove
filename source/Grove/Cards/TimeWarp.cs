namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
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
        .Cast(p =>
          {
            p.Timing = Timings.SecondMain();
            p.Effect = Effect<TargetPlayerTakesExtraTurns>();
            p.EffectTargets = L(Target(Validators.Player(), Zones.None()));
            p.TargetingAi = TargetingAi.Controller();
          });
    }
  }
}