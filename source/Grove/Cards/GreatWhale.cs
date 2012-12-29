namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class GreatWhale : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Great Whale")
        .ManaCost("{5}{U}{U}")
        .Type("Creature Whale")
        .Text("When Great Whale enters the battlefield, untap up to seven lands.")
        .FlavorText("'As a great whale dies, it flips onto its back. And so an island is born.'{EOL}—Mariners' legend")
        .Timing(Timings.Creatures())
        .Power(5)
        .Toughness(5)
        .Abilities(
          TriggeredAbility(
            "When Great Whale enters the battlefield, untap up to seven lands.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<UntapSelectedPermanents>(e =>
              {
                e.Validator = x => x.Is().Land;
                e.MaxCount = 7;
                e.Text = "Select lands to untap";
              })
            )
        );
    }
  }
}