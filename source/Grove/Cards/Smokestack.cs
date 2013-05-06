namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class Smokestack : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Smokestack")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may put a soot counter on Smokestack.{EOL}At the beginning of each player's upkeep, that player sacrifices a permanent for each soot counter on Smokestack.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a soot counter on Smokestack.";
            p.Trigger(new OnStepStart(Step.Upkeep, order: 1));
            p.Effect = () => new ChooseToAddCounter(e => e.Source.OwningCard.Counters <= 3);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each player's upkeep, that player sacrifices a permanent for each soot counter on Smokestack.";

            p.Trigger(new OnStepStart(Step.Upkeep, activeTurn: true, passiveTurn: true));

            p.Effect = () => new PlayersSacrificePermanents(
              count: P(e => e.Source.OwningCard.CountersCount),
              text: "Select permanents to sacrifice.",
              playerFilter: (_, player) => player.IsActive);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}