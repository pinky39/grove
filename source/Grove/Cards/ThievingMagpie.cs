namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class ThievingMagpie : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thieving Magpie")
        .ManaCost("{2}{U}{U}")
        .Type("Creature Bird")
        .Text("{Flying}{EOL}Whenever Thieving Magpie deals damage to an opponent, draw a card.")
        .Power(1)
        .Toughness(3)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Thieving Magpie deals damage to an opponent, draw a card.";
            p.Trigger(new OnDamageDealt(playerFilter: (player, e, dmg) => player == e.Controller.Opponent));
            p.Effect = () => new DrawCards(1);
          });
    }
  }
}