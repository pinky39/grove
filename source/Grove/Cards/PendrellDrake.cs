namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class PendrellDrake : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .Cycling("{2}")
        .StaticAbilities(Static.Flying);
    }
  }
}