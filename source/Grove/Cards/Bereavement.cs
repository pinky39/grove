namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Messages;
  using Core.Zones;

  public class Bereavement : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Bereavement")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Whenever a green creature dies, its controller discards a card.")
        .FlavorText("'Grief is as useless as love.'{EOL}—Gix, Yawgmoth praetor")
        .Cast(p => p.Timing = Timings.FirstMain())
        .Abilities(
          TriggeredAbility(
            "Whenever a green creature dies, its controller discards a card.",
            Trigger<OnZoneChange>(t =>
              {
                t.Filter = (ability, card) => card.Is().Creature && card.HasColors(ManaColors.Green);
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<Core.Cards.Effects.DiscardCards>(p =>
              {
                p.Effect.Count = 1;
                
                p.Effect.Player = p.Parameters
                  .Trigger<CardChangedZone>()
                  .Card.Controller;
              }),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}