namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

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
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your draw step, draw an additional card.",
            Trigger<OnStepStart>(t => { t.Step = Step.Draw; }),
            Effect<DrawCards>(e => { e.Count = 1; }), triggerOnlyIfOwningCardIsInPlay: true),
          TriggeredAbility(
            "At the beginning of your end step, discard your hand.",
            Trigger<OnStepStart>(t => { t.Step = Step.EndOfTurn; }),
            Effect<DiscardHand>(), triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}