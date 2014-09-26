namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class FesteringWound : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Festering Wound")
        .ManaCost("{1}{B}")
        .Type("Enchantment Aura")
        .Text(
          "At the beginning of your upkeep, you may put an infection counter on Festering Wound.{EOL}At the beginning of the upkeep of enchanted creature's controller, Festering Wound deals X damage to that player, where X is the number of infection counters on Festering Wound.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Toughness.GetValueOrDefault(), ControlledBy.Opponent));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put an infection counter on Festering Wound.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddCounters(() => new SimpleCounter(CounterType.Infection), 1));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of the upkeep of enchanted creature's controller, Festering Wound deals X damage to that player, where X is the number of infection counters on Festering Wound.";
            p.Trigger(new OnStepStart(Step.Upkeep, activeTurn: true, passiveTurn: true)
              {
                Condition = (t, g) =>
                  {
                    return t.OwningCard.IsAttached &&
                      t.OwningCard.AttachedTo.Controller.IsActive;
                  }
              });

            p.Effect = () => new DealDamageToPlayer(
              player: P((e, g) => g.Players.Active),
              amount: P(e => e.Source.SourceCard.CountersCount(CounterType.Infection)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}