﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.States;

  public class Waylay : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Waylay")
        .ManaCost("{2}{W}")
        .Type("Instant")
        .Text(
          "Put three Knight tokens into play. Treat these tokens as 2/2 white creatures. Exile them at end of turn.")
        .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
        .Cast(p =>
          {
            p.Effect = () => new CreateTokens(
              count: 3,
              token: Card
                .Named("Knight Token")
                .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
                .Power(2)
                .Toughness(2)
                .OverrideScore(new ScoreOverride {Battlefield = 20})
                .Type("Creature - Token - Knight")
                .Colors(CardColor.White)
                .TriggeredAbility(tp =>
                  {
                    tp.Text = "Exile this at the end of turn.";
                    tp.Trigger(new OnStepStart(
                      step: Step.EndOfTurn,
                      passiveTurn: true,
                      activeTurn: true));
                    tp.Effect = () => new ExileOwner();
                    tp.TriggerOnlyIfOwningCardIsInPlay = true;
                  })
              );
            p.TimingRule(new Any(
              new EndOfTurn(),
              new All(
                new Steps(activeTurn: false, passiveTurn: true, steps: Step.DeclareAttackers),
                new MinAttackerCount(1))));
          });
    }
  }
}