namespace Grove.Cards
{
  using System;
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

  public class BarrinsCodex : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Barrin's Codex")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, put a page counter on Barrin's Codex.{EOL}{4},{T}, Sacrifice Barrin's Codex: Draw X cards, where X is the number of page counters on Barrin's Codex.")
        .Timing(Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, put a page counter on Barrin's Codex.",
            Trigger<AtBegginingOfStep>(t => t.Step = Step.Upkeep),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddCounters>(m => { m.Counter = Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          ActivatedAbility(
            "{4},{T}, Sacrifice Barrin's Codex: Draw X cards, where X is the number of page counters on Barrin's Codex.",
            Cost<PayMana, Tap, Sacrifice>(cost => cost.Amount = 4.Colorless()),
            Effect<DrawCards>(e => e.DrawCount = e.Source.OwningCard.Counters.GetValueOrDefault()),
            timing: Timings.Has3CountersOr1IfDestroyed())
        );
    }
  }
}