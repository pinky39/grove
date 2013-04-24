namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class DarkslickDrake : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Darkslick Drake")
        .ManaCost("{2}{U}{U}")
        .Type("Creature - Drake")
        .Text("{Flying}{EOL}When Darkslick Drake is put into a graveyard from the battlefield, draw a card.")
        .FlavorText("At the edge of the Mephidross, Phyrexia's influence seeps into life and land.")
        .Power(2)
        .Toughness(4)
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Darkslick Drake is put into a graveyard from the battlefield, draw a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Graveyard));
            p.Effect = () => new DrawCards(1);
          });
    }
  }
}