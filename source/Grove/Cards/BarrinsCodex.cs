namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Counters;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class BarrinsCodex : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Barrin's Codex")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, put a page counter on Barrin's Codex.{EOL}{4},{T}, Sacrifice Barrin's Codex: Draw X cards, where X is the number of page counters on Barrin's Codex.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, put a page counter on Barrin's Codex.",
            C.Trigger<AtBegginingOfStep>((t, _) => t.Step = Step.Upkeep),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddCounters>((m, c0) => { m.Counter = c0.Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          C.ActivatedAbility(
            "{4},{T}, Sacrifice Barrin's Codex: Draw X cards, where X is the number of page counters on Barrin's Codex.",
            C.Cost<TapAndSacOwnerPayMana>((cost, _) => cost.Amount = 4.AsColorlessMana()),
            C.Effect<DrawCards>(e => e.DrawCount = e.Source.OwningCard.Counters.GetValueOrDefault()),
            timing: Timings.Has3CountersOr1IfDestroyed())
        );
    }
  }
}