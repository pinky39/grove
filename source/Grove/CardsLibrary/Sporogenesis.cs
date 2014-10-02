namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Modifiers;
  using Triggers;

  public class Sporogenesis : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sporogenesis")
        .ManaCost("{3}{G}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a fungus counter on target nontoken creature.{EOL}Whenever a creature with a fungus counter on it dies, put a 1/1 green Saproling creature token onto the battlefield for each fungus counter on that creature.{EOL}When Sporogenesis leaves the battlefield, remove all fungus counters from all creatures.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a fungus counter on target nontoken creature.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToTargets(() => new AddCounters(() => new SimpleCounter(CounterType.Fungus), 1));

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Creature && !c.Is().Token).On.Battlefield();
                trg.MinCount = 0;
                trg.MaxCount = 1;
              });

            p.TargetingRule(new EffectOrCostRankBy(x => x.Score));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a creature with a fungus counter on it dies, put a 1/1 green Saproling creature token onto the battlefield for each fungus counter on that creature.";

            p.Trigger(
              new OnZoneChanged(
                @from: Zone.Battlefield,
                to: Zone.Graveyard,
                filter: (c, a, g) => c.CountersCount(CounterType.Fungus) > 0));

            p.Effect = () => new CreateTokens(
              count: P(e => e.TriggerMessage<ZoneChangedEvent>().Card.CountersCount(CounterType.Fungus)),
              token: Card
                .Named("Saproling")
                .FlavorText(
                  "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey.")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Saproling")
                .Colors(CardColor.Green));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Sporogenesis leaves the battlefield, remove all fungus counters from all creatures.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new RemoveAllCountersFromPermanents(c => c.Is().Creature, CounterType.Fungus);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}