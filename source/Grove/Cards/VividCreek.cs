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

  public class VividCreek : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Vivid Creek")
        .Type("Land")
        .Text(
          "Vivid Creek enters the battlefield tapped with two charge counters on it.{EOL}{T}: Add {U} to your mana pool.{EOL}{T}, Remove a charge counter from Vivid Creek: Add one mana of any color to your mana pool.")
        .Effect<Core.Cards.Effects.PutIntoPlay>(e => e.PutIntoPlayTapped =  true)
        .Timing(Timings.Lands())
        .Abilities(
          ManaAbility(ManaUnit.Blue, "{T}: Add {U} to your mana pool."),
          ManaAbility(ManaUnit.Any,
            "{T}, Remove a charge counter from Vivid Creek: Add one mana of any color to your mana pool.",
            priority: ManaSourcePriorities.Restricted, cost: Cost<TapOwnerRemoveCounter>()),
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