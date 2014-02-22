namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Triggers;

  public class OrderOfYawgmoth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Order of Yawgmoth")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Zombie Knight")
        .Text("{Fear}{EOL}Whenever Order of Yawgmoth deals damage to a player, that player discards a card.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Fear)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Order of Yawgmoth deals damage to a player, that player discards a card.";
            p.Trigger(new OnDamageDealt(playerFilter: delegate { return true; }));
            p.Effect = () => new DiscardCards(1, P(e => (Player) e.TriggerMessage<DamageHasBeenDealt>().Receiver));
          });
    }
  }
}