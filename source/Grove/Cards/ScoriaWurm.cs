﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.States;

  public class ScoriaWurm : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Scoria Wurm")
        .ManaCost("{4}{R}")
        .Type("Creature Wurm")
        .Text(
          "At the beginning of your upkeep, flip a coin. If you lose the flip, return Scoria Wurm to its owner's hand.")
        .FlavorText(
          "Late at night, ululations echo from deep under Shiv, as the wurms sing of times older than humanity.")
        .Power(7)
        .Toughness(7)
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, flip a coin. If you lose the flip, return Scoria Wurm to its owner's hand.";

            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new FlipACoinReturnToHand();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}