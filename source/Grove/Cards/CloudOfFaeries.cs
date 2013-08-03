namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class CloudOfFaeries : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Cloud of Faeries")
        .Type("Creature Faerie")
        .ManaCost("{1}{U}")
        .Text("{Flying}{EOL}When Cloud of Faeries enters the battlefield, untap up to two lands.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Power(1)
        .Toughness(1)
        .Cycling("{2}")
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Cloud of Faeries enters the battlefield, untap up to two lands.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 2,
              validator: c => c.Is().Land,
              text: "Select lands to untap."
              );
          }
        );
    }
  }
}