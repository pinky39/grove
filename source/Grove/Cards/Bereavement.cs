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
      yield return C.Card
        .Named("Bereavement")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Whenever a green creature dies, its controller discards a card.")
        .FlavorText("'Grief is as useless as love.'{EOL}—Gix, Yawgmoth praetor")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.TriggeredAbility(
            "Whenever a green creature dies, its controller discards a card.",
            C.Trigger<OnZoneChange>((t, _) =>
              {
                t.Filter = (ability, card) => card.Is().Creature && card.HasColors(ManaColors.Green);
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            C.Effect<DiscardCards>(p =>
              p.Effect.ChosenPlayer = p.Parameters
                .Trigger<CardChangedZone>()
                .Card.Controller),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}