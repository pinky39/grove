namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;

  public class GrizzlyBears : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {    
      yield return C.Card
        .Named("Grizzly Bears")
        .ManaCost("{1}{G}")
        .Type("Creature - Bear")
        .FlavorText(
          "We cannot forget that among all of Dominaria's wonders, a system of life exists, with prey and predators that will never fight wars nor vie for ancient power.")
        .Power(2)
        .Toughness(2);
    }
  }
}