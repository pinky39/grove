namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Counters;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;
  using Core.Zones;

  public class VividCreek : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Vivid Creek")
        .Type("Land")
        .Text(
          "Vivid Creek enters the battlefield tapped with two charge counters on it.{EOL}{T}: Add {U} to your mana pool.{EOL}{T}, Remove a charge counter from Vivid Creek: Add one mana of any color to your mana pool.")
        .Cast(p => p.Effect = Effect<Core.Effects.PutIntoPlay>(e => e.PutIntoPlayTapped = true))
        .Abilities(
          ManaAbility(ManaUnit.Blue, "{T}: Add {U} to your mana pool."),
          ManaAbility(ManaUnit.Any,
            "{T}, Remove a charge counter from Vivid Creek: Add one mana of any color to your mana pool.",
            priority: ManaSourcePriorities.Restricted, cost: Cost<Tap, RemoveCounter>()),
          StaticAbility(
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<AddCounters>(m =>
                {
                  m.Count = 2;
                  m.Counter = Counter<ChargeCounter>();
                }))))
        );
    }
  }
}