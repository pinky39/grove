namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class GraftedSkullcap : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Grafted Skullcap")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of your draw step, draw an additional card.{EOL}At the beginning of your end step, discard your hand.")
        .FlavorText("'Let go your mind. Mine is fitter.'{EOL}—Gix, Yawgmoth praetor")
        .Timing(Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your draw step, draw an additional card.",
            Trigger<AtBegginingOfStep>(t => { t.Step = Step.Draw; }),
            Effect<DrawCards>(e =>
              {
                e.DrawCount = 1;                
              }), triggerOnlyIfOwningCardIsInPlay: true),
          TriggeredAbility(
            "At the beginning of your end step, discard your hand.",
            Trigger<AtBegginingOfStep>(t => { t.Step = Step.EndOfTurn; }),
            Effect<DiscardHand>(), triggerOnlyIfOwningCardIsInPlay: true));        
    }
  }
}