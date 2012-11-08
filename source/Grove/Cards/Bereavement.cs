namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
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
        .Timing(Timings.FirstMain())
        .Abilities(
          TriggeredAbility(
            "Whenever a green creature dies, its controller discards a card.",
            Trigger<OnZoneChange>(t =>
              {
                t.Filter = (ability, card) => card.Is().Creature && card.HasColors(ManaColors.Green);
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<DiscardCards>(p =>
              p.Effect.ChosenPlayer = p.Parameters
                .Trigger<CardChangedZone>()
                .Card.Controller),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}