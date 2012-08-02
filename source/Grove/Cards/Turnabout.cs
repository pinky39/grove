namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class Turnabout : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield break;
    }
  }
}