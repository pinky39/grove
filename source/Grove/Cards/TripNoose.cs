namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

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
        .Timing(Timings.FirstMain())
        .Abilities(
          C.ActivatedAbility(
            "{2},{T}: Tap target creature.",
            C.Cost<TapOwnerPayMana>((c, _) =>
              {
                c.Amount = 2.AsColorlessMana();
                c.TapOwner = true;
              }),
            C.Effect<TapTargetCreature>(),
            C.Validator(Validators.Creature()),
            aiTargetFilter: AiTargetSelectors.TapCreature(),
            timing: Timings.Steps(Step.BeginningOfCombat))
        );
    }
  }
}