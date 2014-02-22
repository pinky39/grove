namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class OrderOfTheSacredBell : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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