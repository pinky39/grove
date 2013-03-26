namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class NoeticScales : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Noetic Scales")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of each player's upkeep, return to its owner's hand each creature that player controls with power greater than the number of cards in his or her hand.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each player's upkeep, return to its owner's hand each creature that player controls with power greater than the number of cards in his or her hand.";

            p.Trigger(new OnStepStart(
              step: Step.Upkeep,
              passiveTurn: true,
              activeTurn: true));

            p.Effect = () => new ReturnAllPermanentsToHand((e, c) =>
              c.Is().Creature && c.Controller.IsActive && c.Power > c.Controller.Hand.Count);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}