namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class VividCreek : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.ManaAmount(Mana.Blue);
          })
        .ManaAbility(p =>
          {
            p.Text = "{T}, Remove a charge counter from Vivid Creek: Add one mana of any color to your mana pool.";
            p.Cost = new AggregateCost(new Tap(), new RemoveCounters(CounterType.Charge, count: 1));
            p.ManaAmount(Mana.Any);
            p.Priority = ManaSourcePriorities.Restricted;
          })
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Charge), 2));
            p.UsesStack = false;
          });
    }
  }
}