namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class Uncastable : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Uncastable")
        .ManaCost("{15}")
        .Type("Uncastable")
        .OverrideScore(new ScoreOverride {Battlefield = 0, Graveyard = 0, Exile = 0, Library = 0});
    }
  }
}