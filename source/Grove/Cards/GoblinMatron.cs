namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class GoblinMatron : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin Matron")
        .ManaCost("{2}{R}")
        .Type("Creature Goblin")
        .Text(
          "When Goblin Matron enters the battlefield, you may search your library for a Goblin card, reveal that card, and put it into your hand. If you do, shuffle your library.")
        .FlavorText("There's always room for one more.")
        .Power(1)
        .Toughness(1)
        .Abilities(
          TriggeredAbility(
            "When Goblin Matron enters the battlefield, you may search your library for a Goblin card, reveal that card, and put it into your hand. If you do, shuffle your library.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<SearchLibraryPutToHand>(e =>
              {
                e.MinCount = 0;
                e.MaxCount = 1;
                e.Validator = card => card.Is("goblin");
                e.Text = "Search you library for a goblin card.";
              })));
    }
  }
}