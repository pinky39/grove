namespace Grove.Cards
{
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

  public class VividCreek : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {     
      yield return Card
       .Named("Vivid Creek")
       .Type("Land")
       .Text(
          "Vivid Creek enters the battlefield tapped with two charge counters on it.{EOL}{T}: Add {U} to your mana pool.{EOL}{T}, Remove a charge counter from Vivid Creek: Add one mana of any color to your mana pool.")
       .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
       .ManaAbility(p =>
       {
         p.Text = "{T}: Add {U} to your mana pool.";
         p.ManaAmount(ManaAmount.Blue);
       })
       .ManaAbility(p =>
       {
         p.Text = "{T}, Remove a charge counter from Vivid Creek: Add one mana of any color to your mana pool.";
         p.Cost = new AggregateCost(new Tap(), new RemoveCounter());
         p.ManaAmount(ManaAmount.Any);
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