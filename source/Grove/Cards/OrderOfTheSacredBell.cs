namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;

  public class OrderOfTheSacredBell : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Order of the Sacred Bell")
        .ManaCost("{3}{G}")
        .Type("Creature - Human Monk")
        .FlavorText("'My brother, it may now be time to ring the bell and put out the call for aid.'")
        .Power(4)
        .Toughness(3);
    }
  }
}