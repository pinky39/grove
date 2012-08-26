namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Zones;

  public class Cathodion : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Cathodion")
        .ManaCost("{3}")
        .Type("Artifact Creature Construct")
        .Text("When Cathodion dies, add {3} to your mana pool.")
        .FlavorText(
          "Instead of creating a tool that would be damaged by heat, the Thran built one that was charged by it.")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Abilities(
          C.TriggeredAbility(
            "When Cathodion dies, add {3} to your mana pool.",
            C.Trigger<OnZoneChange>((t, _) =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            C.Effect<AddManaToPool>(e => e.Amount = 3.AsColorlessMana())
            )
        );
    }
  }
}