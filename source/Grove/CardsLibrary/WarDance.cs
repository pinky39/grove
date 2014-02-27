namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class WarDance : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("War Dance")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on War Dance.{EOL}Sacrifice War Dance: Target creature gets +X/+X until end of turn, where X is the number of verse counters on War Dance.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on War Dance.";

            p.Trigger(new OnStepStart(step: Step.Upkeep));

            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(
              () => new SimpleCounter(CounterType.Verse), 1));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice War Dance: Target creature gets +X/+X until end of turn, where X is the number of verse counters on War Dance.";
            p.Cost = new Sacrifice();
            p.Effect = () => new Add11ForEachCounter();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new WhenCardHasCounters(minCount: 3, onlyAtEot: false));
            p.TargetingRule(new EffectPumpInstant(3, 3));
          });
    }
  }
}