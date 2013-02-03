namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class Electryte : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Electryte")
        .ManaCost("{3}{R}{R}")
        .Type("Creature - Beast")
        .Text(
          "Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking creature.")
        .FlavorText("Shivan inhabitants are hardened to fire, so their predators have developed alternative weaponry.")
        .Power(3)
        .Toughness(3)
        .Abilities(
          TriggeredAbility(
            "Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking creature.",
            Trigger<OnDamageDealt>(t =>
              {
                t.CombatOnly = true;
                t.ToPlayer();
              }),
            Effect<DealDamageToCreaturesAndPlayers>(e =>
              {
                e.FilterCreature = (self, card) => card.IsBlocker;
                e.AmountCreature = e.Source.OwningCard.Power.GetValueOrDefault();
              })
            ));
    }
  }
}