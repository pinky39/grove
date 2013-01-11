namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Counters;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class LiltingRefrain : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lilting Refrain")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Lilting Refrain.{EOL}Sacrifice Lilting Refrain: Counter target spell unless its controller pays , where X is the number of verse counters on Lilting Refrain.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you may put a verse counter on Lilting Refrain.",
            Trigger<OnStepStart>(t => t.Step = Step.Upkeep),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddCounters>(m => { m.Counter = Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          ActivatedAbility(
            "Sacrifice Lilting Refrain: Counter target spell unless its controller pays , where X is the number of verse counters on Lilting Refrain.",
            Cost<Sacrifice>(),
            Effect<CounterTargetSpell>(
              e => { e.DoNotCounterCost = e.Source.OwningCard.Counters.GetValueOrDefault().Colorless(); }),
            Target(Validators.CounterableSpell(), Zones.Stack()),
            timing: Timings.CounterSpell(),
            targetingAi: TargetingAi.CounterSpell()));
    }
  }
}