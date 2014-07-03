namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class Uncastable : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Uncastable")
        .ManaCost("{G}{G}{B}{B}{W}{W}{5}")
        .Type("Uncastable")
        .OverrideScore(p =>
          {
            p.Battlefield = 0;
            p.Graveyard = 0;
            p.Exile = 0;
            p.Library = 0;            
          });
    }
  }
}