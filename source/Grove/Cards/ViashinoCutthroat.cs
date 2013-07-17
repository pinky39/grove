namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class ViashinoCutthroat : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Viashino Cutthroat")
        .Type("Creature Viashino")
        .ManaCost("{2}{R}{R}")
        .Text("{Haste}{EOL}At the beginning of the end step, return Viashino Cutthroat to its owner's hand.")
        .FlavorText("You guys go on ahead. I'll catch up with ya. —Vark, goblin scout, last words")
        .Power(5)
        .Toughness(3)
        .SimpleAbilities(Static.Haste)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of the end step, return Viashino Cutthroat to its owner's hand.";
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