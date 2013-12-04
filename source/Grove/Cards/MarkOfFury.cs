﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class MarkOfFury : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mark of Fury")
        .Type("Enchantment Aura")
        .ManaCost("{R}")
        .Text("Enchanted creature has haste.{EOL}At the beginning of the end step, return Mark of Fury to its owner's hand.")
        .FlavorText("Many Keldon warriors bear the mark of intentional insanity.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(              
              () => new AddStaticAbility(Static.Haste));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of the end step, return Mark of Fury to its owner's hand.";
            p.Trigger(new OnStepStart(
              step: Step.EndOfTurn,
              activeTurn: true,
              passiveTurn: true
              ));

            p.Effect = () => new ReturnToHand(returnOwningCard: true);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}