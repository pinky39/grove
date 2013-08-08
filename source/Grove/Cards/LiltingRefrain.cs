namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Costs;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class LiltingRefrain : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lilting Refrain")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Lilting Refrain.{EOL}Sacrifice Lilting Refrain: Counter target spell unless its controller pays , where X is the number of verse counters on Lilting Refrain.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Lilting Refrain.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Verse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice Lilting Refrain: Counter target spell unless its controller pays , where X is the number of verse counters on Lilting Refrain.";
            p.Cost = new Sacrifice();
            p.Effect = () => new CounterTargetSpell(
              doNotCounterCost: P(e => e.Source.OwningCard.Counters));

            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
            p.TimingRule(new Artifical.TimingRules.Counterspell());
            p.TargetingRule(new Artifical.TargetingRules.Counterspell());
          });
    }
  }
}