namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

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

            p.TargetingRule(new EffectRankBy(x => x.Score));
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
              count: P(e => e.TriggerMessage<ZoneChanged>().Card.CountersCount(CounterType.Fungus)),
              token: Card
                .Named("Saproling Token")
                .FlavorText(
                  "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey.")
                .Power(1)
                .Toughness(1)
                .Type("Creature - Token - Saproling")
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