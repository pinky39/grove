namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;

  public class Acridian : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Acridian")
        .ManaCost("{1}{G}")
        .Type("Creature Insect")
        .Text(
          "{Echo} {1}{G}")
        .FlavorText(
          "The elves of Argoth were trained to ride these creatures, even when their mounts traveled upside-down.")
        .Power(2)
        .Toughness(4)
        .Echo("{1}{G}");
    }
  }
}