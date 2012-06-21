namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class AngelicChorus : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Angelic Chorus")
        .ManaCost("{3}{W}{W}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.")
        .FlavorText("The very young and the very old know best the song the angels sing.")
        .Abilities(
          C.TriggeredAbility(
            "Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.",
            C.Trigger<ChangeZone>((t, _) =>
              {
                t.Filter = (ability, card) => ability.Controller == card.Controller && card.Is().Creature;                
                t.To = Zone.Battlefield;
              }),
            C.Effect<GainLifeDependsOnContext<Card>>((e, _) =>
              e.Selector = (card) => card.Toughness.Value), triggerOnlyIfOwningCardIsInPlay: true)              
        );
    }
  }
}