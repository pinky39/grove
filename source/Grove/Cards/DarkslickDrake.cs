namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class DarkslickDrake : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Darkslick Drake")
        .ManaCost("{2}{U}{U}")
        .Type("Creature - Drake")
        .Text("{Flying}{EOL}When Darkslick Drake is put into a graveyard from the battlefield, draw a card.")
        .FlavorText("At the edge of the Mephidross, Phyrexia's influence seeps into life and land.")
        .Power(2)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Abilities(
          StaticAbility.Flying,
          C.TriggeredAbility(
            "When Darkslick Drake is put into a graveyard from the battlefield, draw a card.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Graveyard),
            C.Effect<DrawCards>((e, _) => e.DrawCount = 1)));
    }
  }
}