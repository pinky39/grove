﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class PeregrineDrake : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Peregrine Drake")
        .ManaCost("{4}{U}")
        .Type("Creature Drake")
        .Text("{Flying}{EOL}When Peregrine Drake enters the battlefield, untap up to five lands.")
        .FlavorText("That the Tolarian mists parted for the drakes was warning enough to stay away.")
        .Power(2)
        .Toughness(3)
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Peregrine Drake enters the battlefield, untap up to five lands.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 5,
              validator: c => c.Is().Land,
              text: "Select lands to untap."
              );
          }
        );
    }
  }
}