namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class TuskedColossodon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tusked Colossodon")
        .ManaCost("{4}{G}{G}")
        .Type("Creature - Beast")
        .FlavorText("A band of Temur hunters, fleeing the Mardu, dug a hideout beneath such a creature as it slept. The horde found them and attacked. For three days the Temur held them at bay, and all the while the great beast slumbered.")
        .Power(6)
        .Toughness(5);
    }
  }
}
