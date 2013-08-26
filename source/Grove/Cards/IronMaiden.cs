﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class IronMaiden : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Iron Maiden")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text(
          "At the beginning of each opponent's upkeep, Iron Maiden deals X damage to that player, where X is the number of cards in his or her hand minus 4.")
        .FlavorText("The maiden is a jealous lover.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each opponent's upkeep, Iron Maiden deals X damage to that player, where X is the number of cards in his or her hand minus 4.";

            p.Trigger(new OnStepStart(Step.Upkeep, activeTurn: false, passiveTurn: true));

            p.Effect = () => new DealDamageToPlayer(
              amount: P(e => e.Controller.Opponent.Hand.Count - 4, evaluateOnInit: false, evaluateOnResolve: true),
              player: P(e => e.Controller.Opponent));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}