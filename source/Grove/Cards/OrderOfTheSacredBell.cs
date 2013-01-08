namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class OrderOfTheSacredBell : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
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