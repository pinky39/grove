namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Messages;
  using Core.Triggers;

  public class OrderOfYawgmoth : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Order of Yawgmoth")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Zombie Knight")
        .Text("{Fear}{EOL}Whenever Order of Yawgmoth deals damage to a player, that player discards a card.")
        .Power(2)
        .Toughness(2)
        .StaticAbilities(Static.Fear)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Order of Yawgmoth deals damage to a player, that player discards a card.";
            p.Trigger(new OnDamageDealt(playerFilter: delegate { return true; }));
            p.Effect = () => new DiscardCards(1, P(e => (Player) e.TriggerMessage<DamageHasBeenDealt>().Receiver));
          });
    }
  }
}