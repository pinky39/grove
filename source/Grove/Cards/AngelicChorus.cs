namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Messages;
  using Core.Zones;

  public class AngelicChorus : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Angelic Chorus")
        .ManaCost("{3}{W}{W}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.")
        .FlavorText("The very young and the very old know best the song the angels sing.")
        .Timing(Timings.FirstMain())
        .Abilities(
          TriggeredAbility(
            "Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.",
            Trigger<OnZoneChange>(t =>
              {
                t.Filter = (ability, card) => ability.Controller == card.Controller && card.Is().Creature;
                t.To = Zone.Battlefield;
              }),
            Effect<GainLife>(p =>
              p.Effect.Amount = p.Parameters
                .Trigger<CardChangedZone>()
                .Card.Toughness.GetValueOrDefault()),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}