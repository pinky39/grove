namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Modifiers;
  using Triggers;

  public class UndergrowthScavenger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Undergrowth Scavenger")
        .ManaCost("{3}{G}")
        .Type("Creature — Fungus Horror")
        .Text("Undergrowth Scavenger enters the battlefield with a number of +1/+1 counters on it equal to the number of creature cards in all graveyards.")
        .FlavorText("It sees a rotting carcass as a good wine which has been aged properly.")
        .Power(0)
        .Toughness(0)
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1),
            getCount: (game) => game.Players.Player1.Graveyard.Creatures.Count() + game.Players.Player2.Graveyard.Creatures.Count()));
          p.UsesStack = false;
        });
    }
  }
}
