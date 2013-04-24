﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class CacklingFiend : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Cackling Fiend")
        .ManaCost("{2}{B}{B}")
        .Type("Creature - Zombie")
        .Text("When Cackling Fiend enters the battlefield, each opponent discards a card.")
        .FlavorText("Its windpipe is only the first to amplify its maddening laughter.")
        .Power(2)
        .Toughness(1)
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "When Cackling Fiend enters the battlefield, each opponent discards a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
          });
    }
  }
}