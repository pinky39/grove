namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class LotusBlossom : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lotus Blossom")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.{EOL}{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Petal), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.";

            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new AddManaToPool(P(e =>
              Mana.Colored(ManaColor.Any, e.Source.OwningCard.Counters)));

            p.TimingRule(new WhenYouNeedAdditionalMana());
          });
    }
  }
}