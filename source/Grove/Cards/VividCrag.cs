namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Counters;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Zones;

  public class VividCrag : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Vivid Crag")
        .Type("Land")
        .Text(
          "Vivid Crag enters the battlefield tapped with two charge counters on it.{EOL}{T}: Add {R} to your mana pool.{EOL}{T}, Remove a charge counter from Vivid Crag: Add one mana of any color to your mana pool.")
        .Effect<Core.Cards.Effects.PutIntoPlay>(e => e.PutIntoPlayTapped = true)
        .Timing(Timings.Lands())
        .Abilities(
          ManaAbility(ManaUnit.Red, "{T}: Add {R} to your mana pool."),
          ManaAbility(ManaUnit.Any,
            "{T}, Remove a charge counter from Vivid Crag: Add one mana of any color to your mana pool.",
            priority: ManaSourcePriorities.Restricted, cost: Cost<Tap, RemoveCounter>()),
          StaticAbility(
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<Core.Cards.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<AddCounters>(m =>
                {
                  m.Count = 2;
                  m.Counter = Counter<ChargeCounter>();
                }))))
        );
    }
  }
}