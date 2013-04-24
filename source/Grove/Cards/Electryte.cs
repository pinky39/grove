namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;

  public class Electryte : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking creature.";
            p.Trigger(new OnDamageDealt(combatOnly: true, playerFilter: delegate { return true; }));

            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              filterCreature: (e, card) => card.IsBlocker,
              amountCreature: (e, card) => e.Source.OwningCard.Power.GetValueOrDefault());
          });
    }
  }
}