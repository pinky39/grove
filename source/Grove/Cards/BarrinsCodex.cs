namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class BarrinsCodex : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Barrin's Codex")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, put a page counter on Barrin's Codex.{EOL}{4},{T}, Sacrifice Barrin's Codex: Draw X cards, where X is the number of page counters on Barrin's Codex.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, put a page counter on Barrin's Codex.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Page), 1));
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
            p.Effect = () => new DrawCards(count: P(e => e.Source.OwningCard.Counters));
            p.TimingRule(new WhenCardHasCounters(3));
          });
    }
  }
}