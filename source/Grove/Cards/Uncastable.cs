namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class Uncastable : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Uncastable")
        .ManaCost("{101}")
        .Type("Uncastable")
        .OverrideScore(0)
        .Power(2)
        .Toughness(2);
    }
  }
}