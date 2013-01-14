namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Messages;
  using Core.Triggers;
  using Core.Zones;

  public class Fecundity : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fecundity")
        .ManaCost("{2}{G}")
        .Type("Enchantment")
        .Text("Whenever a creature dies, that creature's controller may draw a card.")
        .FlavorText("Life is eternal. A lifetime is ephemeral.")
        .Cast(p => p.Timing = Timings.FirstMain())        
        .Abilities(
          TriggeredAbility(
            "Whenever a creature dies, that creature's controller may draw a card.",
            Trigger<OnZoneChanged>(t =>
              {
                t.Filter = (ability, card) => card.Is().Creature;
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<Core.Effects.DrawCards>(p =>
              {
                p.Effect.DrawCount = 1;
                
                p.Effect.Player = p.Parameters
                  .Trigger<ZoneChanged>()
                  .Card.Controller;
              }),
            
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}