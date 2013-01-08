namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;

  public class FireAnts : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fire Ants")
        .ManaCost("{2}{R}")
        .Type("Creature Insect")
        .Text("{T}: Fire Ants deals 1 damage to each other creature without flying.")
        .FlavorText("Visitors to Shiv fear the dragons, the goblins, or the viashino. Natives fear the ants.")
        .Power(2)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{T}: Fire Ants deals 1 damage to each other creature without flying.",
            Cost<Tap>(),
            Effect<DealDamageToEach>(e =>
              {
                e.AmountCreature = 1;
                e.FilterCreature = (effect, card) => !card.Has().Flying;
              }),
            timing: Timings.MassRemovalInstantSpeed()
            ));
    }
  }
}