namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Messages;

  public class FleshReaver : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Flesh Reaver")
        .ManaCost("{1}{B}")
        .Type("Creature Horror")
        .Text(
          "Whenever Flesh Reaver deals damage to a creature or opponent, Flesh Reaver deals that much damage to you.")
        .FlavorText(
          "Though the reaver is horrifyingly effective, its dorsal vents spit a highly corrosive cloud of filth.")
        .Power(4)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Abilities(
          C.TriggeredAbility(
            "Whenever Flesh Reaver deals damage to a creature or opponent, Flesh Reaver deals that much damage to you.",
            C.Trigger<DealDamageToCreatureOrPlayer>((t, _) =>
              {
                t.ToAnyCreature();
                t.ToOpponent();
              }),
            C.Effect<DealExistingDamageToController>((e, _) =>
              {
                e.Damage = (effect) =>
                  {
                    var message = effect.Ctx<DamageHasBeenDealt>();
                    return message.Damage;
                  };
              })
            )
        );
    }
  }
}