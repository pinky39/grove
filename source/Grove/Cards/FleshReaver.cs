namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Messages;

  public class FleshReaver : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Flesh Reaver")
        .ManaCost("{1}{B}")
        .Type("Creature Horror")
        .Text(
          "Whenever Flesh Reaver deals damage to a creature or opponent, Flesh Reaver deals that much damage to you.")
        .FlavorText(
          "Though the reaver is horrifyingly effective, its dorsal vents spit a highly corrosive cloud of filth.")
        .Power(4)
        .Toughness(4)
        .Abilities(
          TriggeredAbility(
            "Whenever Flesh Reaver deals damage to a creature or opponent, Flesh Reaver deals that much damage to you.",
            Trigger<OnDamageDealt>(t =>
              {
                t.ToAnyCreature();
                t.ToOpponent();
              }),
            Effect<DealExistingDamageToController>(p =>
              {
                p.Effect.Damage = p.Parameters
                  .Trigger<DamageHasBeenDealt>()
                  .Damage;
              })
            )
        );
    }
  }
}