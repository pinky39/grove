namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Messages;
  using Core.Triggers;

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
            C.Effect<DealDamageToController>((e, _) =>
              {
                e.Amount = (effect) =>
                  {
                    var message = effect.Ctx<DamageHasBeenDealt>();
                    return message.Amount;
                  };
              })
            )
        );
    }
  }
}