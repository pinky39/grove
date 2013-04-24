﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.States;

  public class ReclusiveWight : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Reclusive Wight")
        .ManaCost("{3}{B}")
        .Type("Creature Zombie Minion")
        .Text("At the beginning of your upkeep, if you control another nonland permanent, sacrifice Reclusive Wight.")
        .FlavorText("There are places so horrible that even the dead hide their faces.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if you control another nonland permanent, sacrifice Reclusive Wight.";

            p.Trigger(new OnStepStart(step: Step.Upkeep)
              {
                Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => !c.Is().Land) > 1
              });

            p.Effect = () => new SacrificeSource();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}