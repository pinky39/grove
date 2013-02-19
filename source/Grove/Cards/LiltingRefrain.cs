namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Counters;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class LiltingRefrain : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice Lilting Refrain: Counter target spell unless its controller pays , where X is the number of verse counters on Lilting Refrain.";
            p.Cost = new Sacrifice();
            p.Effect = () => new CounterTargetSpell(
              doNotCounterCost: e => e.Source.OwningCard.Counters.GetValueOrDefault().Colorless());

            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
            p.TimingRule(new Core.Ai.TimingRules.Counterspell());
            p.TargetingRule(new Core.Ai.TargetingRules.Counterspell());
          });
    }
  }
}