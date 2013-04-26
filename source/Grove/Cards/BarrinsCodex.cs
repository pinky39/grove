﻿namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class BarrinsCodex : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Barrin's Codex")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, put a page counter on Barrin's Codex.{EOL}{4},{T}, Sacrifice Barrin's Codex: Draw X cards, where X is the number of page counters on Barrin's Codex.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, put a page counter on Barrin's Codex.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{4},{T}, Sacrifice Barrin's Codex: Draw X cards, where X is the number of page counters on Barrin's Codex.";
            p.Cost = new AggregateCost(
              new PayMana(4.Colorless(), ManaUsage.Abilities),
              new Tap(),
              new Sacrifice());
            p.Effect = () => new DrawCards(count: P(e => e.Source.OwningCard.Counters.GetValueOrDefault()));
            p.TimingRule(new ChargeCounters(3));
          });
    }
  }
}