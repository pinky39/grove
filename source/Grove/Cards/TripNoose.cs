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
        .Cast(p => p.Timing = Timings.FirstMain())
        .Abilities(
          ActivatedAbility(
            "{2},{T}: Tap target creature.",
            Cost<PayMana, Tap>(cost => cost.Amount = 2.Colorless()),
            Effect<TapTarget>(),
            Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()),
            targetingAi: TargetingAi.TapCreature(),
            timing: Timings.Steps(Step.BeginningOfCombat))
        );
    }
  }
}