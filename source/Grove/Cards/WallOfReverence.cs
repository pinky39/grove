﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Player;
  using Gameplay.States;
  using Gameplay.Targeting;

  public class WallOfReverence : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Wall of Reverence")
        .ManaCost("{3}{W}")
        .Type("Creature Spirit Wall")
        .Text(
          "{Defender}, {Flying}{EOL}At the beginning of your end step, you may gain life equal to the power of target creature you control.")
        .FlavorText(
          "The lives of elves are long, but their memories are longer. Even after death, they do not desert their homes.")
        .Power(1)
        .Toughness(6)
        .StaticAbilities(Static.Defender, Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your end step, you may gain life equal to the power of target creature you control.";
            p.Trigger(new OnStepStart(step: Step.EndOfTurn));
            p.Effect = () => new ControllerGainsLife(P(e => e.Target.Card().Power.GetValueOrDefault()));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield());
            p.TargetingRule(new OrderByRank(c => -c.Power.GetValueOrDefault()));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}