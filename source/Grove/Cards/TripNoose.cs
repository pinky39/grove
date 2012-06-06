namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Effects;

  public class TripNoose : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Trip Noose")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text("{2},{T}: Tap target creature.")
        .FlavorText("A taut slipknot trigger is the only thing standing between you and standing.")
        .Timing(Timings.Steps(Step.FirstMain))
        .Abilities(
          C.ActivatedAbility(
            "{2},{T}: Tap target creature.",
            C.Cost<TapOwnerPayMana>((c, _) =>
              {
                c.Amount = 2.AsColorlessMana();
                c.TapOwner = true;
              }),
            C.Effect<TapTargetCreature>(),
            C.Selector(
              target => target.Is().Creature,
              Core.Ai.TargetScores.OpponentStuffScoresMore()
              ),
            timing: Timings.Steps(Step.BeginningOfCombat)
            )
        );
    }
  }
}