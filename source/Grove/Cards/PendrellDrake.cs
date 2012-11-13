namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Dsl;

  public class PendrellDrake : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Pendrell Drake")
        .ManaCost("{3}{U}")
        .Type("Creature - Drake")
        .Text("{Flying}{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .FlavorText(
          "The mages of Tolaria found strange ways to spend their free time. Occasionally they had contests to see whose kite was eaten last.")
        .Power(2)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Cycling("{2}")
        .Abilities(
          Static.Flying
        );
    }
  }
}