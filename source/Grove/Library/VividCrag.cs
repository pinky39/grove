namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class VividCrag : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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