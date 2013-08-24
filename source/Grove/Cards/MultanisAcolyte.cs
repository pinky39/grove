namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class MultanisAcolyte : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Multani's Acolyte")
        .ManaCost("{G}{G}")
        .Type("Creature Elf")
        .Text("{Echo} {G}{G}{EOL}When Multani's Acolyte enters the battlefield, draw a card.")
        .Power(2)
        .Toughness(1)
        .Echo("{G}{G}")
        .TriggeredAbility(p =>
          {
            p.Text = "When Multani's Acolyte enters the battlefield, draw a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DrawCards(1);
          });
    }
  }
}