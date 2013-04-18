namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Counters;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;
  using Core.Zones;

  public class VividCrag : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Vivid Crag")
        .Type("Land")
        .Text(
          "Vivid Crag enters the battlefield tapped with two charge counters on it.{EOL}{T}: Add {R} to your mana pool.{EOL}{T}, Remove a charge counter from Vivid Crag: Add one mana of any color to your mana pool.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} to your mana pool.";
            p.ManaAmount(Mana.Red);
          })
        .ManaAbility(p =>
          {
            p.Text = "{T}, Remove a charge counter from Vivid Crag: Add one mana of any color to your mana pool.";
            p.Cost = new AggregateCost(new Tap(), new RemoveCounter());
            p.ManaAmount(Mana.Any);
            p.Priority = ManaSourcePriorities.Restricted;
          })
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 2));
            p.UsesStack = false;
          });
    }
  }
}