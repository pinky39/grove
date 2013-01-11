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

  public class LotusBlossom : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lotus Blossom")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.{EOL}{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.",
            Trigger<OnStepStart>(t => t.Step = Step.Upkeep),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddCounters>(m => { m.Counter = Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true),
          ManaAbility(
            (self, game) => ManaAmount.OfSingleColor(ManaColors.All, self.OwningCard.Counters.GetValueOrDefault()),
            "{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.",
            Cost<Tap, Sacrifice>())
        );
    }
  }
}