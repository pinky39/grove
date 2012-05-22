namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;

  public class Uncastable : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Uncastable")
        .ManaCost("{42}{G}")
        .Type("Creature - Uncastable")
        .Power(2)
        .Toughness(2);
    }
  }
}