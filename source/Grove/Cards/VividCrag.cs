namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Counters;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;
  using Core.Zones;

  public class VividCrag : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Vivid Crag")
        .Type("Land")
        .Text("Vivid Crag enters the battlefield tapped with two charge counters on it.{EOL}{T}: Add {R} to your mana pool.{EOL}{T}, Remove a charge counter from Vivid Crag: Add one mana of any color to your mana pool.")
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped = (owner) => true)
        .Timing(Timings.Lands())
        .Abilities(
          C.ManaAbility(Mana.Red, "{T}: Add {R} to your mana pool."),
          C.ManaAbility(Mana.Any, "{T}, Remove a charge counter from Vivid Crag: Add one mana of any color to your mana pool.",
            priority: ManaSourcePriorities.Restricted, costFactory: C.Cost<TapOwnerRemoveCounter>()),
          C.StaticAbility(
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<AddCounters>((counter, c1) => {
                counter.Count = 2;
                counter.Counter = c1.Counter<ChargeCounter>();
              }))))
        );
    }
  }
}