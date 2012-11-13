namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class TripNoose : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Trip Noose")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text("{2},{T}: Tap target creature.")
        .FlavorText("A taut slipknot trigger is the only thing standing between you and standing.")
        .Timing(Timings.FirstMain())
        .Abilities(
          ActivatedAbility(
            "{2},{T}: Tap target creature.",
            Cost<TapOwnerPayMana>(cost =>
              {
                cost.Amount = 2.AsColorlessMana();
                cost.TapOwner = true;
              }),
            Effect<TapTargetCreature>(),
            Validator(Validators.Creature()),
            selectorAi: TargetSelectorAi.TapCreature(),
            timing: Timings.Steps(Step.BeginningOfCombat))
        );
    }
  }
}