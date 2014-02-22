namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class Uncastable : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Uncastable")
        .ManaCost("{G}{G}{B}{B}{W}{W}{5}")
        .Type("Uncastable")
        .OverrideScore(new ScoreOverride {Battlefield = 0, Graveyard = 0, Exile = 0, Library = 0});
    }
  }
}