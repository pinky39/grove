namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class Uncastable : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Uncastable")
        .ManaCost("{42}{G}")
        .Type("Creature - Uncastable")
        .OverrideScore(0)
        .Power(2)
        .Toughness(2);
    }
  }
}