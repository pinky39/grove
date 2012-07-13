namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class VolcanicHammer : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Volcanic Hammer")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Volcanic Hammer deals 3 damage to target creature or player.")
        .FlavorText("Fire finds its form in the heat of the forge.")
        .Effect<DealDamageToTarget>((e, _) => e.SetAmount(3))
        .Timing(Timings.MainPhases())
        .Targets(filter: TargetFilters.DealDamage(3),
          effect: C.Selector(
            Selectors.CreatureOrPlayer()));
    }
  }
}