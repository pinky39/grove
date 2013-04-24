namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;

  public class Uncastable : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Uncastable")
        .ManaCost("{G}{G}{B}{B}{W}{W}{5}")
        .Type("Uncastable")
        .OverrideScore(new ScoreOverride {Battlefield = 0, Graveyard = 0, Exile = 0, Library = 0});
    }
  }
}