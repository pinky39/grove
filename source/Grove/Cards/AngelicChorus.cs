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
      yield return C.Card
        .Named("Angelic Chorus")
        .ManaCost("{3}{W}{W}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.")
        .FlavorText("The very young and the very old know best the song the angels sing.")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.TriggeredAbility(
            "Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.",
            C.Trigger<ChangeZone>((t, _) =>
              {
                t.Filter = (ability, card) => ability.Controller == card.Controller && card.Is().Creature;
                t.To = Zone.Battlefield;
              }),
            C.Effect<GainLife>((e, _) => e.Amount = ef => ef.Ctx<CardChangedZone>().Card.Toughness.Value),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}