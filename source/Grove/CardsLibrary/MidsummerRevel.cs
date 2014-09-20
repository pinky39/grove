namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class MidsummerRevel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Midsummer Revel")
        .ManaCost("{3}{G}{G}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Midsummer Revel.{EOL}{G},Sacrifice Midsummer Revel: Put X 3/3 green Beast creature tokens onto the battlefield, where X is the number of verse counters on Midsummer Revel.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Midsummer Revel.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Verse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{G},Sacrifice Midsummer Revel: Put X 3/3 green Beast creature tokens onto the battlefield, where X is the number of verse counters on Midsummer Revel.";
            p.Cost = new AggregateCost(
              new PayMana(Mana.Green, ManaUsage.Abilities),
              new Sacrifice());
            p.Effect = () => new CreateTokens(
              count: P(e => e.Source.OwningCard.Counters),
              token: Card
                .Named("Beast Token")
                .FlavorText(
                  "All we know about the Krosan Forest we have learned from those few who made it out alive.")
                .Power(3)
                .Toughness(3)
                .Type("Creature Token Beast")
                .Colors(CardColor.Green));

            p.TimingRule(new WhenCardHasCounters(3));
          });
    }
  }
}