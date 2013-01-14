namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class HorseshoeCrab : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Horseshoe Crab")
        .ManaCost("{2}{U}")
        .Type("Creature - Crab")
        .Text("{U}: Untap Horseshoe Crab.")
        .FlavorText("In the final days before the disaster, all the crabs on Tolaria migrated from inlets, streams, and ponds back to the sea. No one took note.")
        .Power(1)
        .Toughness(3)
        .Abilities(
          ActivatedAbility(
            "{U}: Untap Horseshoe Crab.",
            Cost<PayMana>(cost => cost.Amount = ManaAmount.Blue),
            Effect<UntapOwner>(),
            timing: All(Timings.Has(x => x.IsTapped), Timings.MainPhases(), Timings.Turn(active: true))));
    }
  }
}